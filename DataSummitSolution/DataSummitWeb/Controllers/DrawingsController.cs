using DataSummitModels;
using DataSummitREST;
using DataSummitWeb.DTO;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataSummitWeb.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class DrawingsController : Controller
    {
        //DataSummitHelper.Users usersService = new DataSummitHelper.Users(new DataSummitDbContext());
        DataSummitHelper.AzureCompanyResourceUrls azureService = new DataSummitHelper.AzureCompanyResourceUrls(new DataSummitDbContext());
        DataSummitHelper.Drawings drawingsService = new DataSummitHelper.Drawings(new DataSummitDbContext());
        DataSummitHelper.ProfileVersions templatesService = new DataSummitHelper.ProfileVersions(new DataSummitDbContext());

        private DataSummitDbContext db = new DataSummitDbContext();
        // GET api/drawings/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            List<DataSummitModels.Drawings> lDrawings = drawingsService.GetAllCompanyDrawings(id);
            return JsonConvert.SerializeObject(lDrawings.ToArray());
        }

        [HttpGet("templates/{id}")]
        public string GetTemplates(int companyId)
        {
            List<DataSummitModels.ProfileVersions> lTemplates = templatesService.GetAllCompanyProfileVersions(companyId);
            return JsonConvert.SerializeObject(lTemplates.ToArray());
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody] List<DrawingUpload> drawings)
        {
            List<long> returnIds = new List<long>();
            try
            {
                List<Drawings> drawingsOut = new List<Drawings>();
                //Catches empty UI data pass
                if (drawings == null) return "Invalid drawing upload";

                ////Verify credentials prior to processing
                //AspNetUsers uC = db.AspNetUsers.First(u => u.Id == drawings.First().UserId);
                //if (uC.IsTrial == true)
                //{
                //    //Selects the maximum number of drawings based on their current Trial Limit
                //    drawings = drawings.Take(TrialRemaining(uC)).ToList();
                //    //Need to modify UI to handle this type of call
                //    if (drawings.Count == 0) return "Drawing Limit Hit";
                //}
                //else
                //{
                //    //Company based account

                //    //Tests required:
                //    //Monthly quota limits (contract)
                //    //Monthly quota limits (PAYG)
                //    //Bundle limits and expiration (prepaid bundles)
                //}

                //List<Task> lTasks = new List<Task>();
                foreach (DrawingUpload du in drawings)
                {
                    //lTasks.Add(Task.Run(() =>
                    //{
                    if (du.File != null)
                    {
                        List<Drawings> dOut = ProcessDrawingUpload(du);
                        drawingsOut.AddRange(dOut);
                    }
                    //}));
                }
                foreach(Drawings d in drawingsOut)
                {
                    long id = drawingsService.CreateDrawing(drawingsOut);
                    if (id > 0) returnIds.Add(id);
                }
                //Task.WaitAll(lTasks.ToArray());
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }

            return JsonConvert.SerializeObject(returnIds);
        }

        // PUT api/drawings/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Drawings project)
        {
            //Update
            drawingsService.UpdateDrawing(id, project);
            return;
        }

        // DELETE api/drawings/5
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            drawingsService.DeleteDrawing(id);
            return JsonConvert.SerializeObject("Ok");
        }

        private List<Drawings> ProcessDrawingUpload (DrawingUpload du)
        {
            List<Drawings> lRet = new List<Drawings>();
            try
            {
                ImageUpload imU = new ImageUpload();
                imU.ProjectId = du.ProjectId;
                imU.ProfileVersionId = du.TemplateId;
                imU.FileName = du.FileName;
                imU.File = du.File;
                //imU.UserId = uC.Id;

                Projects cProject = db.Projects.First(p => p.ProjectId == du.ProjectId);

                string s = Convert.ToBase64String(du.File);
                if (AuditDataPassed(ref imU) == true)
                {

                    if (du.FileType == "application/pdf")
                    {
                        imU.Type = "PDF";
                        lRet.AddRange(ProcessPDF(imU, cProject));
                    }
                    else if (du.FileType == "image/jpeg")
                    {
                        imU.Type = "JPG";
                        lRet.AddRange(ProcessDrawings(imU));
                    }
                    else if (du.FileType == "image/x-png")
                    {
                        imU.Type = "PNG";
                        lRet.AddRange(ProcessDrawings(imU));
                    }
                    else if (du.FileType == "image/gif")
                    {
                        imU.Type = "GIF";
                        lRet.AddRange(ProcessDrawings(imU));
                    }
                }
            }
            catch (Exception ae)
            {
                string strMessage = ae.Message.ToString();
                string strInner = ae.InnerException.ToString();
            }
            return lRet;
        }

        private bool AuditDataPassed(ref ImageUpload imgU)
        {
            try
            {
                int pId = imgU.ProjectId;
                //Check for project number
                if (pId <= 0) return false;
                //Check for company number, acquire from project if necessary
                if (imgU.ProjectId > 0)
                {
                    var comp = db.Companies.FirstOrDefault(c => c.Projects.Count(p => p.ProjectId == pId) > 0);
                    imgU.CompanyId = comp.CompanyId;
                }
                if (imgU.CompanyId <= 0) return false;

            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return true;
        }

        private int TrialRemaining(AspNetUsers user)
        {
            try
            {
                int dCount = db.Drawings.Count(d => d.UserId == user.Id);
                //Remaining trial documents
                if (dCount > 0 && dCount < 100)
                {
                    return 100 - dCount;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ae)
            { string strError = ae.Message.ToString(); }
            return 0;
        }

        private List<DataSummitModels.Drawings> ProcessPDF(ImageUpload drawData, Projects cProject)
        {
            List<DataSummitModels.Drawings> draws = new List<Drawings>();
            try
            {
                List<ImageUpload> lFiles = new List<ImageUpload>();

                Uri uriSplitDocument = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.SplitDocument);
                drawData.BlobStorageName = cProject.BlobStorageName;
                drawData.BlobStorageKey = cProject.BlobStorageKey;
                drawData.CompanyId = cProject.CompanyId;

                HttpResponseMessage httpPDFImages = API.ProcessCall(uriSplitDocument, JsonConvert.SerializeObject(drawData));
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
            {
                string strError = ae.Message.ToString();
            }
            return draws;
        }

        private List<Drawings> ProcessDrawings(ImageUpload drawData)
        {
            List<Drawings> draws = new List<Drawings>();
            try
            {
                List<ImageUpload> lFiles = new List<ImageUpload>();
                Projects cProject = db.Projects.First(p => p.ProjectId == drawData.ProjectId);
                List<AzureCompanyResourceUrls> lRes = db.AzureCompanyResourceUrls.Where(a => a.CompanyId == cProject.CompanyId).ToList();
                drawData.BlobStorageName = cProject.BlobStorageName;
                drawData.BlobStorageKey = cProject.BlobStorageKey;
                drawData.CompanyId = cProject.CompanyId;

                //Document manipulation
                Uri uriSplitDocument = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.SplitDocument);
                Uri uriImageToContainer = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.ImageToContainer);
                Uri uriDivideImage = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.DivideImage);
                //OCR
                //Uri uriAmazonOCR = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.RecogniseTextAmazon);
                Uri uriAzureOCR = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.RecogniseTextAzure);
                //Uri uriGoogleOCR = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.RecogniseTextGoogle);
                //Post processing
                Uri uriPostProcessing = azureService.GetIndividualUrl(drawData.CompanyId, DataSummitHelper.AzureCompanyResourceUrls.AzureResource.PostProcessing);

                if (drawData.Type == "PDF")
                {
                    //Ensure that each PDF is split into a single drawing
                    HttpResponseMessage httpPDFImages = API.ProcessCall(uriSplitDocument, JsonConvert.SerializeObject(drawData));
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

                    ////Iterate the list of PDFs
                    //for (int i = 0; i < lPDFImages.Count; i++)
                    //{
                    //    HttpResponseMessage httpImageUpload = API.ProcessCall(uriImageToContainer, JsonConvert.SerializeObject(lPDFImages[i]));
                    //    try
                    //    {
                    //        string sImageUpload = httpImageUpload.Content.ReadAsStringAsync().Result;
                    //        ImageUpload imgResponse = JsonConvert.DeserializeObject<DataSummitModels.ImageUpload>(sImageUpload);
                    //        lPDFImages[i] = imgResponse;
                    //    }
                    //    catch (Exception ae1)
                    //    { string strError = ae1.ToString(); }
                    //}
                }
                else
                {
                    //Single image file, no manipulation required
                    lFiles.Add(drawData);
                }


                //List<Task> tFunctions = new List<Task>();

                //Upload, split and OCR each image
                for (int i = 0; i < lFiles.Count; i++)
                {
                    //tFunctions.Add(Task.Run(() =>
                    //{ 
                    //Upload image to Azure storage and create container if necessary
                    HttpResponseMessage httpImageUpload = API.ProcessCall(uriImageToContainer, JsonConvert.SerializeObject(lFiles[i]));
                    string sImageUpload = httpImageUpload.Content.ReadAsStringAsync().Result;
                    ImageUpload respImageUpload = JsonConvert.DeserializeObject<ImageUpload>(sImageUpload);
                    respImageUpload.Tasks = VerifyTaskList(respImageUpload.Tasks.ToList());
                    lFiles[i] = respImageUpload;

                    //Divide image into OCR acceptable sives
                    HttpResponseMessage httpDivideImage = API.ProcessCall(uriDivideImage, JsonConvert.SerializeObject(lFiles[i]));
                    string sDivideImage = httpDivideImage.Content.ReadAsStringAsync().Result;
                    ImageUpload respDivideImage = JsonConvert.DeserializeObject<ImageUpload>(sDivideImage);
                    respDivideImage.Tasks = VerifyTaskList(respDivideImage.Tasks.ToList());
                    lFiles[i] = respDivideImage;

                    ///Self cleaning occurs in all 3 OCR functions
                    byte iAmazonIt = 0; byte iAzureIt = 0; byte iGoogleIh = 0;
                    //Azure
                    while (iAzureIt < 10 && lFiles[i].SplitImages.Count(si => si.ProcessedAzure == false) > 0)
                    {
                        lFiles[i] = StartPostProcessing(uriAzureOCR, lFiles[i]);
                        iAzureIt = (byte)(iAzureIt + 1);
                    }
                    //List<Task> tOCRs = new List<Task>();
                    //List<ImageUpload> asyncI = new List<ImageUpload>();
                    //tOCRs.Add(Task.Run(async () => 
                    //{
                    //    ////Amazon OCR
                    //    //asyncI.Add(await StartPostProcessingAsync(uriAmazonOCR, lFiles[i]));
                    //    //Azure OCR
                    //    asyncI.Add(await StartPostProcessingAsync(uriAzureOCR, lFiles[i]));
                    //    ////Google OCR
                    //    //asyncI.Add(await StartPostProcessingAsync(uriGoogleOCR, lFiles[i]));
                    //}));
                    ////Wait for all results
                    //Task.WaitAll(tOCRs.ToArray());
                }
                //Task.WaitAll(tFunctions.ToArray());

                ///Cross vendor cleaning needs to occur here as a new and separate Azure Function


                //Convert ImageUpload object data to a Drawing object data
                foreach (ImageUpload f in lFiles)
                {
                    if (f.WidthOriginal >= f.HeightOriginal)
                    { f.PaperOrientationId = 2; }
                    else
                    { f.PaperOrientationId = 1; }
                    f.CreatedDate = DateTime.Now;
                    draws.Add(f.ToDrawing());
                }
                //}
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return draws;
        }

        private ImageUpload StartPostProcessing(Uri uriPostProcessing, ImageUpload im)
        {
            ImageUpload imOut = im;
            try
            {
                //Filter and augment OCR results
                HttpResponseMessage httpPostProcessing = API.ProcessCall(uriPostProcessing, JsonConvert.SerializeObject(imOut));
                string sPostProcessing = httpPostProcessing.Content.ReadAsStringAsync().Result;
                ImageUpload respPostProcessing = JsonConvert.DeserializeObject<ImageUpload>(sPostProcessing);
                imOut = respPostProcessing;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return imOut;
        }

        private async Task<ImageUpload> StartPostProcessingAsync(Uri uriPostProcessing, ImageUpload im)
        {
            ImageUpload imOut = im;
            try
            {
                //Filter and augment OCR results
                HttpResponseMessage httpPostProcessing = API.ProcessCall(uriPostProcessing, JsonConvert.SerializeObject(imOut));
                string sPostProcessing = await httpPostProcessing.Content.ReadAsStringAsync();
                ImageUpload respPostProcessing = JsonConvert.DeserializeObject<ImageUpload>(sPostProcessing);
                imOut = respPostProcessing;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return imOut;
        }

        private List<Tasks> VerifyTaskList(List<Tasks> lTasks)
        {
            try
            {
                if (lTasks == null) lTasks = new List<Tasks>();
                if (lTasks.Count == 0)
                {
                    foreach (Tasks td in lTasks)
                    { lTasks.Add(td); };
                    if (lTasks.Count == 0)
                    {
                        lTasks.Add(
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
            {
                string strError = ae.Message.ToString();
                if (ae.InnerException != null) strError = ae.InnerException.ToString();
            }
            return lTasks;
        }
    }
}
