using DataSummitHelper.Dao.Interfaces;
using DataSummitHelper.Interfaces;
using DataSummitModels.DTO;
using DataSummitModels.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitHelper.Services
{
    public class UploadService
    {
        private readonly IDataSummitDao _dao;
        private readonly IDataSummitHelperService _dataSummitHelper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public UploadService(IDataSummitDao dao, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _dao = dao;
            _configuration = configuration;
        }

        public async Task<List<DocumentUpload>> ProcessUploadedFileAsync(IFormFile file)
        {
            List<DocumentUpload> docs = new List<DocumentUpload>();
            try
            {
                var documentClassification = _configuration["Upload"];
                Uri uriImageToContainer = _dataSummitHelper.GetIndividualUrl(1, Azure.Functions.ImageToContainer.ToString());
                if (IsAcceptedFormat(file.Name))
                {
                    DataSummitModels.Enums.Document.Format format = GetFormat(file.Name);
                    var doc = new DocumentUpload();

                    //Upload image to Azure storage and create container if necessary
                    var httpImageUpload = await _dataSummitHelper.ProcessCall(uriImageToContainer, JsonConvert.SerializeObject(file));
                    string docUploadString = await httpImageUpload.Content.ReadAsStringAsync();
                    var docUploadObject = JsonConvert.DeserializeObject<DocumentUpload>(docUploadString);
                    if (docUploadObject != null)
                    { docs.Add(docUploadObject);  }
                }
            }
            catch (Exception)
            { }
            return docs;
        }

        private bool IsAcceptedFormat(string filename)
        {
            try
            {
                if (filename.Length > 0)
                {
                    string extension = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf(".")).ToLower();
                    if (extension == ".jpg" || extension == ".png" || extension == ".pdf")
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            { }
            return false;
        }

        private DataSummitModels.Enums.Document.Format GetFormat(string filename)
        {
            try
            {
                string extension = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf(".")).ToLower();
                if (extension == ".pdf")
                {
                    return DataSummitModels.Enums.Document.Format.PDF;
                }
                else if (extension == ".jpg")
                {
                    return DataSummitModels.Enums.Document.Format.JPG;
                }
                else if (extension == ".png")
                {
                    return DataSummitModels.Enums.Document.Format.PNG;
                }

                //var mimeType = string.Empty;
                //switch (documentUpload.FileType)
                //{
                //    case "application/pdf":
                //        mimeType = Document.Format.PDF.ToString();
                //        break;
                //    case "image/jpeg":
                //        mimeType = Document.Format.JPG.ToString();
                //        break;
                //    case "image/x-png":
                //        mimeType = Document.Format.PNG.ToString();
                //        break;
                //    case "image/gif":
                //        mimeType = Document.Format.GIF.ToString();
                //        break;
                //}
            }
            catch (Exception)
            { }
            return DataSummitModels.Enums.Document.Format.Unknown;
        }


    }
}