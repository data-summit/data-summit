using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using DataSummitHelper.Interfaces;
using DataSummitHelper.Interfaces.MachineLearning;
using DataSummitModels.DB;
using DataSummitModels.DTO;
using DataSummitModels.Enums;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;



namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public partial class DocumentsController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;
        private readonly IAzureResourcesService _azureResources;
        private readonly IClassificationService _classificationService;

        public DocumentsController(IDataSummitHelperService dataSummitHelper, IAzureResourcesService azureResources,
            IClassificationService classificationService)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
            _azureResources = azureResources;
            _classificationService = classificationService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var documents = await _dataSummitHelper.GetProjectDocuments(id);
            return Ok(documents);
        }

        [HttpGet("templates/{id}")]
        public async Task<IActionResult> GetTemplates(int companyId)
        {
            var templates = await _dataSummitHelper.GetAllCompanyTemplates(companyId);
            return Ok(templates);
        }

        [HttpPost]
        public async Task<HashSet<string>> UploadFiles(ICollection<IFormFile> files)
        {
            var uploadedFileURLs = new HashSet<string>();
            try
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        var uploadedFileUrl = await _azureResources.UploadDataToBlob(file);
                        var typeResults = await _classificationService.DocumentType(uploadedFileUrl, "DocumentType", "Classification");
                        uploadedFileURLs.Add(uploadedFileUrl);
                    }
                }
            }
            catch (Exception ae)
            {
                //TODO log exception
                return null;
            }
            return uploadedFileURLs;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] DataSummitModels.DB.Document project)
        {
            //Update
            //_dataSummitHelper.UpdateDocument(id, project);
            return;
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //_dataSummitHelper.DeleteDocument(id);
            return JsonConvert.SerializeObject("Ok");
        }

        private List<DataSummitModels.DB.Document> ProcessDocumentUpload(DocumentUpload documentUpload)
        {
            var documents = new List<DataSummitModels.DB.Document>();
            try
            {
                if (AuditUploadIds(documentUpload))
                {
                    var mimeType = DataSummitModels.Enums.Document.Format.Unknown;
                    //switch (documentUpload.DocumentFormat)
                    switch ("")
                    {
                        case "application/pdf":
                            mimeType = DataSummitModels.Enums.Document.Format.PDF;
                            break;
                        case "image/jpeg":
                            mimeType = DataSummitModels.Enums.Document.Format.JPG;
                            break;
                        case "image/x-png":
                            mimeType = DataSummitModels.Enums.Document.Format.PNG;
                            break;
                        case "image/gif":
                            mimeType = DataSummitModels.Enums.Document.Format.GIF;
                            break;
                    }

                    var imageUpload = new ImageUpload
                    {
                        ProjectId = documentUpload.ProjectId,
                        TemplateVersionId = documentUpload.TemplateId,
                        FileName = documentUpload.FileName,
                        Format = mimeType
                    };
                    MemoryStream ms = new MemoryStream();
                    documentUpload.File.OpenReadStream().CopyTo(ms);
                    imageUpload.File = ms.ToArray();


                    documents.AddRange(ProcessDocuments(imageUpload));
                }
            }
            catch (Exception ae)
            { }

            return documents;
        }

        private bool AuditUploadIds(DocumentUpload imgU)
        {
            bool idsAreValid = false;

            idsAreValid &= imgU.ProjectId > 0;
            // idsAreValid &= imgU.CompanyId > 0;

            return idsAreValid;
        }

        private List<DataSummitModels.DB.Document> ProcessPDF(ImageUpload drawData, Project cProject)
        {
            List<DataSummitModels.DB.Document> documentss = new List<DataSummitModels.DB.Document>();
            try
            {
                List<ImageUpload> lFiles = new List<ImageUpload>();

                Uri uriSplitDocument = _dataSummitHelper.GetIndividualUrl(drawData.CompanyId, ""); //AzureFunctions.SplitDocument.ToString());
                drawData.StorageAccountName = cProject.StorageAccountName;
                drawData.StorageAccountKey = cProject.StorageAccountKey;
                drawData.CompanyId = cProject.CompanyId;

                HttpResponseMessage httpPDFImages = ProcessCall(uriSplitDocument, JsonConvert.SerializeObject(drawData));
                List<ImageUpload> lPDFImages = new List<ImageUpload>();
                try
                {
                    string sPDFImages = httpPDFImages.Content.ReadAsStringAsync().Result;
                    lPDFImages = JsonConvert.DeserializeObject<List<ImageUpload>>(sPDFImages);
                    if (lPDFImages.Count > 0)
                    { lFiles.AddRange(lPDFImages); }
                }
                catch (Exception ae1)
                { string strError = ae1.ToString(); }
            }
            catch (Exception ae)
            { }

            return documentss;
        }

        private List<DataSummitModels.DB.Document> ProcessDocuments(ImageUpload imageUpload)
        {
            List<DataSummitModels.DB.Document> documentss = new List<DataSummitModels.DB.Document>();
            try
            {
                //    List<ImageUpload> imageUploads = new List<ImageUpload>();
                //    Project projects = null;
                //    imageUpload.StorageAccountName = projects.StorageAccountName;
                //    imageUpload.StorageAccountKey = projects.StorageAccountKey;
                //    imageUpload.CompanyId = projects.CompanyId;

                //    //Document manipulation
                //    Uri uriSplitDocument = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.SplitDocument.ToString());
                //    Uri uriImageToContainer = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.ImageToContainer.ToString());
                //    Uri uriDivideImage = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.DivideImage.ToString());

                //    //OCR
                //    Uri uriAzureOCR = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.RecogniseTextAzure.ToString());

                //    //Post processing
                //    Uri uriPostProcessing = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.PostProcessing.ToString());

                //    //Extract title block properties
                //    Uri uriExtractTitleBlock = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.ExtractTitleBlock.ToString());

                //    if (imageUpload.Format == DataSummitModels.Enums.Document.Format.PDF)
                //    {
                //        //Ensure that each PDF is split into a single document
                //        var httpPDFImages = ProcessCall(uriSplitDocument, JsonConvert.SerializeObject(imageUpload));
                //        var pdfImages = new List<ImageUpload>();
                //        try
                //        {
                //            string pdfImageStrings = httpPDFImages.Content.ReadAsStringAsync().Result;
                //            pdfImages = JsonConvert.DeserializeObject<List<ImageUpload>>(pdfImageStrings);
                //            if (pdfImages.Count > 0)
                //            { imageUploads.AddRange(pdfImages); }
                //        }
                //        catch (Exception ae1)
                //        { }
                //    }
                //    else
                //    {
                //        //Single image file, no manipulation required
                //        imageUploads.Add(imageUpload);
                //    }

                //    //Upload, split, OCR and list title block properties for each image
                //    for (int i = 0; i < imageUploads.Count; i++)
                //    {
                //        //Upload image to Azure storage and create container if necessary
                //        var httpImageUpload = ProcessCall(uriImageToContainer, JsonConvert.SerializeObject(imageUploads[i]));
                //        string imageUploadString = httpImageUpload.Content.ReadAsStringAsync().Result;
                //        var imageUploadObject = JsonConvert.DeserializeObject<ImageUpload>(imageUploadString);
                //        imageUploadObject.Tasks = imageUploadObject.Tasks.ToList();
                //        imageUploads[i] = imageUploadObject;

                //        //Divide image into OCR acceptable sives
                //        var divideImage = ProcessCall(uriDivideImage, JsonConvert.SerializeObject(imageUploads[i]));
                //        string divideImageString = divideImage.Content.ReadAsStringAsync().Result;
                //        var divideImageUploadObject = JsonConvert.DeserializeObject<ImageUpload>(divideImageString);
                //        divideImageUploadObject.Tasks = divideImageUploadObject.Tasks.ToList();
                //        imageUploads[i] = divideImageUploadObject;

                //        ///Self cleaning occurs in all 3 OCR functions
                //        byte iAzureIt = 0;

                //        //Azure
                //        while (iAzureIt < 10 && imageUploads[i].SplitImages.Count(si => si.ProcessedAzure == false) > 0)
                //        {
                //            imageUploads[i] = StartPostProcessing(uriAzureOCR, imageUploads[i]);
                //            iAzureIt = (byte)(iAzureIt + 1);
                //        }

                //        //Extract title block properties
                //        imageUploads[i] = ExtractTitleBlockProperties(uriExtractTitleBlock, imageUploads[i]);
                //    }

                //    //Convert ImageUpload object data to a Document object data
                //    foreach (ImageUpload f in imageUploads)
                //    {
                //        if (f.WidthOriginal >= f.HeightOriginal)
                //        { f.PaperOrientationId = 2; }
                //        else
                //        { f.PaperOrientationId = 1; }
                //        f.CreatedDate = DateTime.Now;
                //        documentss.Add(f.ToDocument());
                //    }
            }
            catch (Exception ae)
            { }
            return documentss;
        }

        private ImageUpload StartPostProcessing(Uri uriPostProcessing, ImageUpload im)
        {
            ImageUpload imOut = im;
            try
            {
                //Filter and augment OCR results
                HttpResponseMessage httpPostProcessing = ProcessCall(uriPostProcessing, JsonConvert.SerializeObject(imOut));
                string sPostProcessing = httpPostProcessing.Content.ReadAsStringAsync().Result;
                ImageUpload respPostProcessing = JsonConvert.DeserializeObject<ImageUpload>(sPostProcessing);
                imOut = respPostProcessing;
            }
            catch (Exception ae)
            { }
            return imOut;
        }

        private ImageUpload ExtractTitleBlockProperties(Uri uriExtractTitleBlock, ImageUpload im)
        {
            ImageUpload imOut = im;
            try
            {
                //Extract title block properties from attributes and sentences
                HttpResponseMessage httpExtractTitleBlock = ProcessCall(uriExtractTitleBlock, JsonConvert.SerializeObject(imOut));
                string sExtractTitleBlock = httpExtractTitleBlock.Content.ReadAsStringAsync().Result;
                ImageUpload respExtractTitleBlock = JsonConvert.DeserializeObject<ImageUpload>(sExtractTitleBlock);
                respExtractTitleBlock.Tasks = respExtractTitleBlock.Tasks.ToList();
                imOut = respExtractTitleBlock;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return imOut;
        }


        public static HttpResponseMessage ProcessCall(Uri uri, string payload)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();

            HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

            var stringTask = client.PostAsync(uri, httpContent);
            return stringTask.Result;
        }
    }
}