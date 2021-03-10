using DataSummitModels.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataSummitHelper.Interfaces
{
    public interface IUploadService
    {
        Task<List<DocumentUpload>> ProcessUploadedFileAsync(IFormFile file);

    }
}
