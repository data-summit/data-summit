using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
using DataSummitWeb.DTO;
using DataSummitModels.Enums;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataSummitWeb.Enums;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public partial class DocumentsController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;
        private const int maxTrialDocumentUploads = 100;

        private enum ImageUploadTypes
        {
            GIF,
            JPG,
            JPEG,
            PDF,
            PNG
        }

        public DocumentsController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
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
        public string Post([FromBody] List<DocumentUpload> documentUploads)
        {
            var returnIds = new List<long>();
            try
            {
                var documents = new List<Documents>();
                if (documents == null)
                {
                    return "Invalid document upload";
                }

                foreach (DocumentUpload documentUpload in documentUploads)
                {
                    if (documentUpload.File != null)
                    {
                        var processedDocument = ProcessDocumentUpload(documentUpload);
                        documents.AddRange(processedDocument);
                    }
                }

                foreach(Documents d in documents)
                {
                    // long id = _dataSummitHelper.CreateDocument(documents);
                    // if (id > 0) returnIds.Add(id);
                }
            }
            catch (Exception ae)
            { }

            return JsonConvert.SerializeObject(returnIds);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Documents project)
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

        private List<Documents> ProcessDocumentUpload (DocumentUpload documentUpload)
        {
            var documents = new List<Documents>();
            try
            {
                if (AuditUploadIds(documentUpload))
                {
                    var mimeType = Document.Format.Unknown;
                    switch (documentUpload.FileType)
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
                        ProfileVersionId = documentUpload.TemplateId,
                        FileName = documentUpload.FileName,
                        File = documentUpload.File,
                        Format = mimeType
                    };

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

        private int TrialRemaining()
        {
            int trialRemainingDocumentCount = 0;

            try
            {
                //Remaining trial documents
                trialRemainingDocumentCount = trialRemainingDocumentCount > 0 && trialRemainingDocumentCount < maxTrialDocumentUploads 
                    ? 100 - trialRemainingDocumentCount 
                    : trialRemainingDocumentCount;
            }
            catch (Exception ae)
            { }

            return trialRemainingDocumentCount;
        }

        private List<Documents> ProcessPDF(ImageUpload drawData, Projects cProject)
        {
            List<Documents> documentss = new List<Documents>();
            try
            {
                List<ImageUpload> lFiles = new List<ImageUpload>();

                Uri uriSplitDocument = _dataSummitHelper.GetIndividualUrl(drawData.CompanyId, Azure.Functions.SplitDocument.ToString());
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

        private List<Documents> ProcessDocuments(ImageUpload imageUpload)
        {
            List<Documents> documentss = new List<Documents>();
            try
            {
                List<ImageUpload> imageUploads = new List<ImageUpload>();
                Projects projects = null;
                imageUpload.StorageAccountName = projects.StorageAccountName;
                imageUpload.StorageAccountKey = projects.StorageAccountKey;
                imageUpload.CompanyId = projects.CompanyId;

                //Document manipulation
                Uri uriSplitDocument = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.SplitDocument.ToString());
                Uri uriImageToContainer = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.ImageToContainer.ToString());
                Uri uriDivideImage = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.DivideImage.ToString());
                
                //OCR
                Uri uriAzureOCR = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.RecogniseTextAzure.ToString());
                
                //Post processing
                Uri uriPostProcessing = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.PostProcessing.ToString());
                
                //Extract title block properties
                Uri uriExtractTitleBlock = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, Azure.Functions.ExtractTitleBlock.ToString());

                if (imageUpload.Format == DataSummitModels.Enums.Document.Format.PDF)
                {
                    //Ensure that each PDF is split into a single document
                    var httpPDFImages = ProcessCall(uriSplitDocument, JsonConvert.SerializeObject(imageUpload));
                    var pdfImages = new List<ImageUpload>();
                    try
                    {
                        string pdfImageStrings = httpPDFImages.Content.ReadAsStringAsync().Result;
                        pdfImages = JsonConvert.DeserializeObject<List<ImageUpload>>(pdfImageStrings);
                        if (pdfImages.Count > 0)
                        { imageUploads.AddRange(pdfImages); }
                    }
                    catch (Exception ae1)
                    { }
                }
                else
                {
                    //Single image file, no manipulation required
                    imageUploads.Add(imageUpload);
                }

                //Upload, split, OCR and list title block properties for each image
                for (int i = 0; i < imageUploads.Count; i++)
                {
                    //Upload image to Azure storage and create container if necessary
                    var httpImageUpload = ProcessCall(uriImageToContainer, JsonConvert.SerializeObject(imageUploads[i]));
                    string imageUploadString = httpImageUpload.Content.ReadAsStringAsync().Result;
                    var imageUploadObject = JsonConvert.DeserializeObject<ImageUpload>(imageUploadString);
                    imageUploadObject.Tasks = VerifyTaskList(imageUploadObject.Tasks.ToList());
                    imageUploads[i] = imageUploadObject;

                    //Divide image into OCR acceptable sives
                    var divideImage = ProcessCall(uriDivideImage, JsonConvert.SerializeObject(imageUploads[i]));
                    string divideImageString = divideImage.Content.ReadAsStringAsync().Result;
                    var divideImageUploadObject = JsonConvert.DeserializeObject<ImageUpload>(divideImageString);
                    divideImageUploadObject.Tasks = VerifyTaskList(divideImageUploadObject.Tasks.ToList());
                    imageUploads[i] = divideImageUploadObject;

                    ///Self cleaning occurs in all 3 OCR functions
                    byte iAzureIt = 0;

                    //Azure
                    while (iAzureIt < 10 && imageUploads[i].SplitImages.Count(si => si.ProcessedAzure == false) > 0)
                    {
                        imageUploads[i] = StartPostProcessing(uriAzureOCR, imageUploads[i]);
                        iAzureIt = (byte)(iAzureIt + 1);
                    }

                    //Extract title block properties
                    imageUploads[i] = ExtractTitleBlockProperties(uriExtractTitleBlock, imageUploads[i]);
                }

                //Convert ImageUpload object data to a Document object data
                foreach (ImageUpload f in imageUploads)
                {
                    if (f.WidthOriginal >= f.HeightOriginal)
                    { f.PaperOrientationId = 2; }
                    else
                    { f.PaperOrientationId = 1; }
                    f.CreatedDate = DateTime.Now;
                    documentss.Add(f.ToDocument());
                }
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
                respExtractTitleBlock.Tasks = VerifyTaskList(respExtractTitleBlock.Tasks.ToList());
                imOut = respExtractTitleBlock;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return imOut;
        }

        // TODO: This method makes absolutely NO SENSE!
        private List<Tasks> VerifyTaskList(List<Tasks> tasks)
        {
            var taskList = tasks ?? new List<Tasks>();
            try
            {
                if (taskList.Count == 0)
                {
                    foreach (var task in taskList)
                    { taskList.Add(task); };
                    
                    if (tasks.Count == 0)
                    {
                        tasks.Add(
                          new Tasks
                          {
                              Name = "Azure Task Count byPass (to be deleted)",
                              TaskId = (long)1,
                              TimeStamp = DateTime.Now
                          }
                        );
                    }
                }
            }
            catch (Exception ae)
            { }
            return tasks;
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
