﻿using Azure.Storage.Blobs.Specialized;
using DataSummitService.Dto;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSummitService.Interfaces
{
    public interface IAzureResourcesService
    {
        //Resource
        Task<string> CreateResourceGroup(string name);
        Task<string> UpdateResourceGroup(string oldName, string newName);
        Task<bool> DeleteResourceGroup(string name);

        //Storage Account
        Task<string> CreateStorageAccount(string resourceGroup, string name);
        Task<string> UpdateStorageAccount(string resourceGroup, string oldName, string newName);
        Task<bool> DeleteStorageAccount(string resourceGroup, string name);

        //Block blob
        Task<FileUploadSummaryDto> UploadDataToBlob(IFormFile file);
        BlockBlobClient GetBlobByUrl(string blobUrl);
        Task AddMetadataToBlob(string blobUrl, List<KeyValuePair<string, string>> metadata);
    }
}
