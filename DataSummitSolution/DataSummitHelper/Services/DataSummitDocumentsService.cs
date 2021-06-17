
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitService.Dao.Interfaces;
using DataSummitService.Dto;
using DataSummitService.Interfaces;
using DataSummitService.Interfaces.MachineLearning;
using DataSummitModels.DB;
using DataSummitModels.Enums;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Azure.Storage.Blobs.Models;

namespace DataSummitService.Services
{
    public class DataSummitDocumentsService : IDataSummitDocumentsService
    {
        private readonly IDataSummitHelperService _dataSummitHelper;
        private readonly IDataSummitDocumentsDao _documentsDao;
        private readonly IDataSummitTemplateAttributesDao _templateAttributesDao;
        private readonly IDataSummitAzureUrlsDao _azureDao;
        private readonly IAzureResourcesService _azureResourcesService;

        public DataSummitDocumentsService(IAzureResourcesService azureResourcesService,
                                          IDataSummitHelperService dataSummitHelper, 
                                          IDataSummitDocumentsDao documentsDao,
                                          IDataSummitTemplateAttributesDao templateAttributesDao,
                                          IDataSummitAzureUrlsDao azureDao)
        {

            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _documentsDao = documentsDao ?? throw new ArgumentNullException(nameof(documentsDao)); 
            _templateAttributesDao = templateAttributesDao ?? throw new ArgumentNullException(nameof(templateAttributesDao));
            _azureDao = azureDao ?? throw new ArgumentNullException(nameof(azureDao));
            _azureResourcesService = azureResourcesService ?? throw new ArgumentNullException(nameof(azureResourcesService));
        }

        public DocumentContentType DocumentType(string mimeType)
        {
            var enumType = DocumentContentType.Unknown;
            switch (mimeType)
            {
                case "DrawingPlanView":
                    enumType = DocumentContentType.DrawingPlanView;
                    break;
                case "Gantt":
                    enumType = DocumentContentType.Gantt;
                    break;
                case "Report":
                    enumType = DocumentContentType.Report;
                    break;
                case "Schematic":
                    enumType = DocumentContentType.Schematic;
                    break;
            }
            return enumType;
        }

        public DocumentExtension DocumentFormat(string mimeFormat)
        {
            var enumFormat = DocumentExtension.Unknown;
            switch (mimeFormat)
            {
                case "application/pdf":
                    enumFormat = DocumentExtension.PDF;
                    break;
                case "image/jpeg":
                    enumFormat = DocumentExtension.JPG;
                    break;
                case "image/x-png":
                    enumFormat = DocumentExtension.PNG;
                    break;
                case "image/gif":
                    enumFormat = DocumentExtension.GIF;
                    break;
            }
            return enumFormat;
        }

        /// <summary>
        ///  This is tech-debt and in a future ticket/iteration we should put the object(string) in a class and have 
        ///  a ToEnum() method in that class.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns>DrawingLayout enum</returns>
        public DrawingLayout GetDrawingLayoutEnum(string itemName)
        {
            var enumComponent = DrawingLayout.Unknown;
            switch (itemName)
            {
                case "DrawingContent":
                    enumComponent = DrawingLayout.DrawingContent;
                    break;
                case "Notes":
                    enumComponent = DrawingLayout.Notes;
                    break;
                case "TitleBox":
                    enumComponent = DrawingLayout.TitleBox;
                    break;
            }
            return enumComponent;
        }

        public async Task<ImageDimensionsDto> GetDocumentImageDimensionsFromMetadata(string blobUrl)
        {
            var dimensions = new ImageDimensionsDto();
            var metadata = await _azureResourcesService.GetBlobMetadata(blobUrl);    
            
            //Verify that blob metadata fields are populated 
            if (metadata.Count(m => m.Key == "IsImage") > 0 &&
                metadata.Count(m => m.Key == "Width") > 0 &&
                metadata.Count(m => m.Key == "Height") > 0)
            {
                //Verify that blob 'Is an image'
                if (metadata.Count(m => m.Key == "IsImage" && m.Value == "true") > 0)
                {
                    int.TryParse(metadata.Single(m => m.Key == "Width").Value, out int width);
                    int.TryParse(metadata.Single(m => m.Key == "Height").Value, out int height);

                    //Validate dimension values
                    if (width > 0 && height > 0)
                    {
                        dimensions.Width = width;
                        dimensions.Height = height;
                    }
                }
            }

            return dimensions;
        }

        public async Task<KeyValuePair<string, int>> GetDocumentText(string blobUrl)
        {
            var features = new List<DocumentFeature>();

            // Find image dimensions, needed for making positions relavtive (between 0 & 1) not absolute
            var imageDimenions = await GetDocumentImageDimensionsFromMetadata(blobUrl);

            // Fetch resource url & key information from database
            var azureCognitiveServiceOCR = await _azureDao.GetAzureResourceKeyPairByNameAndType("RecogniseTextAzure", "Cognitive Services");

            // Subscription key required as a header, not as a url parameter
            var headers = new Dictionary<string, string>
            {
                { "Ocp-Apim-Subscription-Key", azureCognitiveServiceOCR.Key }
            };

            //TODO catch error responses
            var httpResponse = await _dataSummitHelper.ProcessCall(
                uri: new Uri(azureCognitiveServiceOCR.Url + "?language=unk&detectOrientation=true"),
                payload: "{ \"url\": \"" + blobUrl + "\"}", 
                headers: headers);

            var response = await httpResponse.Content.ReadAsStringAsync();
            var detectedText = JsonConvert.DeserializeObject<DataSummitModels.Cloud.CognitiveServices.OCR.Application>(response);

            // Convert OCR object to document feature object
            foreach (var region in detectedText.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        string[] dimensions = word.BoundingBox.Split(",");
                        int.TryParse(dimensions[0], out int left);
                        int.TryParse(dimensions[1], out int top);
                        int.TryParse(dimensions[2], out int width);
                        int.TryParse(dimensions[3], out int height);

                        var documentFeature = new DocumentFeature
                        {
                            Confidence = 1,
                            Feature = "Text",
                            Height = Math.Round(height / (decimal)imageDimenions.Height, 5),
                            Left = Math.Round(left / (decimal)imageDimenions.Width, 5),
                            Top = Math.Round(top / (decimal)imageDimenions.Height, 5),
                            Width = Math.Round(width / (decimal)imageDimenions.Width, 5),
                            Vendor = "Azure",
                            Value = word.Text,
                        };
                        features.Add(documentFeature);
                    }
                }
            }
            // Add new features to database
            await _documentsDao.UpdateDocumentFeatures(blobUrl, features);

            return new KeyValuePair<string, int>(blobUrl, features.Count);
        }

        public DocumentDto GetDocumentDtoByUrl(string documentUrl)
        {
            var document = _documentsDao.GetDocumentByUrl(documentUrl);
            var documentDto = new DocumentDto(document);
            return documentDto;
        }

        public Document GetDocumentByUrl(string documentUrl) => _documentsDao.GetDocumentByUrl(documentUrl);

        public async Task<List<DocumentDto>> GetDocumentsForProjectId(int projectId)
        {
            var documents = await _documentsDao.GetAllProjectDocuments(projectId);

            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }

        public async Task<List<DocumentDto>> GetProjectDocuments(int projectId)
        {
            var documents = await _documentsDao.GetProjectDocuments(projectId);
            var documentDtos = documents.Select(d => new DocumentDto(d))
                .ToList();

            return documentDtos;
        }

        public async Task DeleteDocumentProperty(long documentPropertyId)
        {
            await _templateAttributesDao.DeleteTemplateAttribute(documentPropertyId);
        }

        public void UpdateDocument(Document document)
        {
            _documentsDao.UpdateDocument(document);
        }
    }
}