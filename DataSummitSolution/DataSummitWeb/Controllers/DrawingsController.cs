using DataSummitHelper.Interfaces;
using DataSummitModels.DB;
using DataSummitWeb.DTO;
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
    public partial class DrawingsController : Controller
    {
        private readonly IDataSummitHelperService _dataSummitHelper;
        private const int maxTrialDrawingUploads = 100;

        private enum ImageUploadTypes
        {
            GIF,
            JPG,
            JPEG,
            PDF,
            PNG
        }

        private enum AzureResource
        {
            SplitDocument = 1,
            ImageToContainer = 2,
            DivideImage = 3,
            RecogniseTextAzure = 4,
            PostProcessing = 5,
            ExtractTitleBlock = 6
        }

        public DrawingsController(IDataSummitHelperService dataSummitHelper)
        {
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var drawings = await _dataSummitHelper.GetProjectDrawings(id);
            return Ok(drawings);
        }

        [HttpGet("templates/{id}")]
        public async Task<IActionResult> GetTemplates(int companyId)
        {
            var templates = await _dataSummitHelper.GetAllCompanyTemplates(companyId);
            return Ok(templates);
        }

        [HttpPost]
        public string Post([FromBody] List<DrawingUpload> drawingUploads)
        {
            var returnIds = new List<long>();
            try
            {
                var drawings = new List<Drawings>();
                if (drawings == null)
                {
                    return "Invalid drawing upload";
                }

                foreach (DrawingUpload drawingUpload in drawingUploads)
                {
                    if (drawingUpload.File != null)
                    {
                        var processedDrawing = ProcessDrawingUpload(drawingUpload);
                        drawings.AddRange(processedDrawing);
                    }
                }

                foreach(Drawings d in drawings)
                {
                    // long id = _dataSummitHelper.CreateDrawing(drawings);
                    // if (id > 0) returnIds.Add(id);
                }
            }
            catch (Exception ae)
            { }

            return JsonConvert.SerializeObject(returnIds);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Drawings project)
        {
            //Update
            //_dataSummitHelper.UpdateDrawing(id, project);
            return;
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            //_dataSummitHelper.DeleteDrawing(id);
            return JsonConvert.SerializeObject("Ok");
        }

        private List<Drawings> ProcessDrawingUpload (DrawingUpload drawingUpload)
        {
            var drawings = new List<Drawings>();
            try
            {
                if (AuditUploadIds(drawingUpload))
                {
                    var mimeType = string.Empty;
                    switch (drawingUpload.FileType)
                    {
                        case "application/pdf":
                            mimeType = ImageUploadTypes.PDF.ToString();
                            break;
                        case "image/jpeg":
                            mimeType = ImageUploadTypes.JPG.ToString();
                            break;
                        case "image/x-png":
                            mimeType = ImageUploadTypes.PNG.ToString();
                            break;
                        case "image/gif":
                            mimeType = ImageUploadTypes.GIF.ToString();
                            break;
                    }

                    var imageUpload = new ImageUpload
                    {
                        ProjectId = drawingUpload.ProjectId,
                        ProfileVersionId = drawingUpload.TemplateId,
                        FileName = drawingUpload.FileName,
                        File = drawingUpload.File,
                        Type = mimeType
                    };

                    drawings.AddRange(ProcessDrawings(imageUpload));
                }
            }
            catch (Exception ae)
            { }

            return drawings;
        }

        private bool AuditUploadIds(DrawingUpload imgU)
        {
            bool idsAreValid = false;

            idsAreValid &= imgU.ProjectId > 0;
            // idsAreValid &= imgU.CompanyId > 0;

            return idsAreValid;
        }

        private int TrialRemaining()
        {
            int trialRemainingDrawingCount = 0;

            try
            {
                //Remaining trial documents
                trialRemainingDrawingCount = trialRemainingDrawingCount > 0 && trialRemainingDrawingCount < maxTrialDrawingUploads 
                    ? 100 - trialRemainingDrawingCount 
                    : trialRemainingDrawingCount;
            }
            catch (Exception ae)
            { }

            return trialRemainingDrawingCount;
        }

        private List<Drawings> ProcessPDF(ImageUpload drawData, Projects cProject)
        {
            List<Drawings> drawingss = new List<Drawings>();
            try
            {
                List<ImageUpload> lFiles = new List<ImageUpload>();

                Uri uriSplitDocument = _dataSummitHelper.GetIndividualUrl(drawData.CompanyId, AzureResource.SplitDocument.ToString());
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

            return drawingss;
        }

        private List<Drawings> ProcessDrawings(ImageUpload imageUpload)
        {
            List<Drawings> drawingss = new List<Drawings>();
            try
            {
                List<ImageUpload> imageUploads = new List<ImageUpload>();
                Projects projects = null;
                imageUpload.StorageAccountName = projects.StorageAccountName;
                imageUpload.StorageAccountKey = projects.StorageAccountKey;
                imageUpload.CompanyId = projects.CompanyId;

                //Document manipulation
                Uri uriSplitDocument = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, AzureResource.SplitDocument.ToString());
                Uri uriImageToContainer = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, AzureResource.ImageToContainer.ToString());
                Uri uriDivideImage = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, AzureResource.DivideImage.ToString());
                
                //OCR
                Uri uriAzureOCR = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, AzureResource.RecogniseTextAzure.ToString());
                
                //Post processing
                Uri uriPostProcessing = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, AzureResource.PostProcessing.ToString());
                
                //Extract title block properties
                Uri uriExtractTitleBlock = _dataSummitHelper.GetIndividualUrl(imageUpload.CompanyId, AzureResource.ExtractTitleBlock.ToString());

                if (imageUpload.Type == ImageUploadTypes.PDF.ToString())
                {
                    //Ensure that each PDF is split into a single drawing
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

                //Convert ImageUpload object data to a Drawing object data
                foreach (ImageUpload f in imageUploads)
                {
                    if (f.WidthOriginal >= f.HeightOriginal)
                    { f.PaperOrientationId = 2; }
                    else
                    { f.PaperOrientationId = 1; }
                    f.CreatedDate = DateTime.Now;
                    drawingss.Add(f.ToDrawing());
                }
            }
            catch (Exception ae)
            { }
            return drawingss;
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
