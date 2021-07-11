using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using DataSummitService.Dao.Interfaces;
using DataSummitService.Interfaces;
using DataSummitModels.DB;
using DataSummitModels.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.Management.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataSummitService.Dto;
using System.Web;

namespace DataSummitService.Services
{
    public class AzureResourcesService : IAzureResourcesService
    {
        private readonly IConfiguration _configuration;
        private readonly IDataSummitDocumentsDao _documentsDao;
        private readonly IDataSummitHelperService _dataSummitHelper;

        public AzureResourcesService(IDataSummitHelperService dataSummitHelper, IDataSummitDocumentsDao documentsDao, IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _documentsDao = documentsDao ?? throw new ArgumentNullException(nameof(documentsDao));
            _dataSummitHelper = dataSummitHelper ?? throw new ArgumentNullException(nameof(dataSummitHelper));
        }

        #region Management Client Access
        private ResourceManagementClient GetResourceManagementAuthorizationToken()
        {
            ResourceManagementClient resourceClient = default;
            try
            {
                ServicePrincipalLoginInformation spli = new ServicePrincipalLoginInformation();
                spli.ClientId = _configuration["ApplicationClientId"];
                spli.ClientSecret = _configuration["ServicePrincipalSecret"];
                AzureCredentials creds = new AzureCredentials(spli,
                    _configuration["TenantId"], AzureEnvironment.AzureGlobalCloud);

                // Build the service credentials and Azure Resource Manager clients
                RestClient client = RestClient
                    .Configure()
                    .WithEnvironment(AzureEnvironment.AzureGlobalCloud)
                    .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                    .WithCredentials(creds)
                    .Build();

                resourceClient = new ResourceManagementClient(client)
                {
                    SubscriptionId = _configuration["AzureSubscriptionId"]
                };
            }
            catch (Exception)
            {; }
            return resourceClient;
        }
        private StorageManagementClient GetStorageManagementAuthorizationToken()
        {
            StorageManagementClient resourceClient = default;
            try
            {
                ServicePrincipalLoginInformation spli = new ServicePrincipalLoginInformation();
                spli.ClientId = _configuration["ApplicationClientId"];
                spli.ClientSecret = _configuration["ServicePrincipalSecret"];
                AzureCredentials creds = new AzureCredentials(spli,
                    _configuration["TenantId"], AzureEnvironment.AzureGlobalCloud);

                // Build the service credentials and Azure Resource Manager clients
                RestClient client = RestClient
                    .Configure()
                    .WithEnvironment(AzureEnvironment.AzureGlobalCloud)
                    .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                    .WithCredentials(creds)
                    .Build();

                resourceClient = new StorageManagementClient(creds)
                {
                    SubscriptionId = _configuration["AzureSubscriptionId"]
                };
            }
            catch (Exception)
            {; }
            return resourceClient;
        }
        #endregion

        #region Resource Groups
        public async Task<string> CreateResourceGroup(string name)
        {
            string nameOut = "";
            try
            {
                //Convert name to allowable Azure Resource Group name
                name = name.Replace(" ", "");
                name = Regex.Replace(name, @"^[a-zA-Z0-9_-]*$", "", RegexOptions.None);
                ResourceManagementClient resourceClient = GetResourceManagementAuthorizationToken();
                //Loop while a name can be found that doesn't exist
                int i = 0;
                bool NameExists = true;
                while (NameExists == true)
                {
                    nameOut = "";
                    if (i == 0)
                    {
                        NameExists = await resourceClient.ResourceGroups.CheckExistenceAsync(name);
                    }
                    else
                    {
                        NameExists = await resourceClient.ResourceGroups.CheckExistenceAsync(name + i.ToString("0000"));
                        nameOut = name + i.ToString("0000");
                    }
                    i = i + 1;
                }

                var groupParams = new ResourceGroupInner();
                groupParams.Location = "uksouth";
                groupParams.Tags = new Dictionary<string, string> { { "Created", DateTime.Now.ToString() } };
                await resourceClient.ResourceGroups.CreateOrUpdateAsync(nameOut, groupParams);
                resourceClient.Dispose();
            }
            catch (Exception)
            {; }
            return nameOut;
        }
        public async Task<string> UpdateResourceGroup(string oldName, string newName)
        {
            string nameOut = "";
            try
            {
                //Convert name to allowable Azure Resource Group name
                newName = newName.Replace(" ", "");
                newName = Regex.Replace(newName, @"^[a-zA-Z0-9_-]*$", "", RegexOptions.None);

                ResourceManagementClient resourceClient = GetResourceManagementAuthorizationToken();
                //Loop while a name can be found that doesn't exist
                int i = 0;
                bool Exists = await resourceClient.ResourceGroups.CheckExistenceAsync(oldName);
                if (Exists)
                {
                    while (Exists == true)
                    {
                        nameOut = "";
                        if (i == 0)
                        {
                            Exists = await resourceClient.ResourceGroups.CheckExistenceAsync(newName);
                            if (Exists == false)
                            {
                                nameOut = newName;
                            }
                        }
                        else
                        {
                            Exists = await resourceClient.ResourceGroups.CheckExistenceAsync(newName + i.ToString("0000"));
                            if (Exists == false)
                            {
                                nameOut = newName + i.ToString("0000");
                            }
                        }
                        i = i + 1;
                    }

                    //Get existing Resource Group
                    var oldRG = await resourceClient.ResourceGroups.GetAsync(oldName);
                    //Transfer metadata to updated Resource Group
                    var rgp = new ResourceGroupPatchable()
                    {
                        ManagedBy = oldRG.ManagedBy,
                        Name = oldRG.Name,
                        Properties = oldRG.Properties,
                        Tags = oldRG.Tags

                    };
                    //Update Resource Group
                    await resourceClient.ResourceGroups.UpdateAsync(nameOut, rgp);
                    resourceClient.Dispose();
                }
            }
            catch (Exception)
            {; }
            return nameOut;
        }
        public async Task<bool> DeleteResourceGroup(string name)
        {
            bool WasDeleted = false;
            try
            {
                ResourceManagementClient resourceClient = GetResourceManagementAuthorizationToken();
                bool Exists = await resourceClient.ResourceGroups.CheckExistenceAsync(name);
                if (Exists)
                {
                    await resourceClient.ResourceGroups.DeleteAsync(name);
                    if (resourceClient.ResourceGroups.ListAsync().Result.Count(n => n.Name == name) == 0)
                    {
                        WasDeleted = true;
                    }
                }
                resourceClient.Dispose();
            }
            catch (Exception)
            {; }
            return WasDeleted;
        }
        #endregion

        #region Storage Accounts
        public async Task<string> CreateStorageAccount(string resourceGroup, string name)
        {
            string nameOut = "";
            try
            {
                //Convert name to allowable Azure Storage Account name
                name = name.Replace(" ", "");
                name = Regex.Replace(name, @"^[a-zA-Z0-9]*$", "", RegexOptions.None);
                if (name.Length < 3)
                {
                    name = name + new string('0', name.Length - 3);
                }
                if (name.Length > 24)
                {
                    name = name.Substring(0, 23);
                }
                name = name.ToLower();

                StorageManagementClient resourceClient = GetStorageManagementAuthorizationToken();
                //Get Storage Account in relevant Resource Group
                var sap = new Microsoft.Azure.Management.Storage.Models.StorageAccountCreateParameters();
                sap.Tags = new Dictionary<string, string> { { "Created", DateTime.Now.ToString() } };

                //Loop while a name can be found that doesn't exist
                int i = 0;
                bool NameExists = true;
                while (NameExists == true)
                {
                    nameOut = "";
                    if (i == 0)
                    {
                        var avail = await resourceClient.StorageAccounts.CheckNameAvailabilityAsync(name + i.ToString("0000"));
                        if ((bool)avail.NameAvailable)
                        {
                            NameExists = false;
                            nameOut = name;
                        }
                        else
                        {
                            name = name.Substring(0, 19);
                        }
                    }
                    else
                    {
                        var avail = await resourceClient.StorageAccounts.CheckNameAvailabilityAsync(name + i.ToString("0000"));
                        if ((bool)avail.NameAvailable)
                        {
                            NameExists = false;
                            nameOut = name + i.ToString("0000");
                        }
                    }
                    i = i + 1;
                }

                var groupParams = new ResourceGroupInner();
                groupParams.Location = "uksouth";
                groupParams.Tags = new Dictionary<string, string> { { "Created", DateTime.Now.ToString() } };
                var sa = await resourceClient.StorageAccounts.CreateAsync(resourceGroup, name, sap);
                resourceClient.Dispose();
            }
            catch (Exception)
            { return null; }
            return nameOut;
        }
        public async Task<string> UpdateStorageAccount(string resourceGroup, string oldName, string newName)
        {
            string nameOut = "";
            try
            {
                //Convert newName to allowable Azure Storage Account newName
                newName = newName.Replace(" ", "");
                newName = Regex.Replace(newName, @"^[a-zA-Z0-9]*$", "", RegexOptions.None);
                if (newName.Length < 3)
                {
                    newName = newName + new string('0', newName.Length - 3);
                }
                if (newName.Length > 24)
                {
                    newName = newName.Substring(0, 23);
                }
                newName = newName.ToLower();

                StorageManagementClient resourceClient = GetStorageManagementAuthorizationToken();
                //Get existing Storage Account properties
                var sa = await resourceClient.StorageAccounts.GetPropertiesAsync(resourceGroup, oldName);
                int i = 0;
                bool Exists = true;
                if (sa != null)
                {
                    nameOut = newName;
                    while (Exists == true)
                    {
                        nameOut = "";
                        if (i == 0)
                        {
                            var IsAvailable = await resourceClient.StorageAccounts.CheckNameAvailabilityAsync(newName);
                            if ((bool)IsAvailable.NameAvailable)
                            {
                                Exists = false;
                                nameOut = newName;
                            }
                            else
                            {
                                newName = newName.Substring(0, 19);
                            }
                        }
                        else
                        {
                            var IsAvailable = await resourceClient.StorageAccounts.CheckNameAvailabilityAsync(newName + i.ToString("0000"));
                            if ((bool)IsAvailable.NameAvailable)
                            {
                                Exists = false;
                                nameOut = newName + i.ToString("0000");
                            }
                        }
                        i = i + 1;
                    }

                    //Transfer metadata to updated Resource Group
                    var saup = new Microsoft.Azure.Management.Storage.Models.StorageAccountUpdateParameters()
                    {
                        Tags = sa.Tags,
                    };
                    //Update Resource Group
                    var saNew = await resourceClient.StorageAccounts.UpdateAsync(resourceGroup, nameOut, saup);
                    resourceClient.Dispose();
                }
            }
            catch (Exception)
            { return null; }
            return nameOut;
        }
        public async Task<bool> DeleteStorageAccount(string resourceGroup, string name)
        {
            bool WasDeleted = false;
            try
            {
                StorageManagementClient resourceClient = GetStorageManagementAuthorizationToken();
                var IsAvailable = await resourceClient.StorageAccounts.CheckNameAvailabilityAsync(name);
                bool Exists = (bool)IsAvailable.NameAvailable;
                if (Exists == false)
                {
                    await resourceClient.StorageAccounts.DeleteAsync(resourceGroup, name);
                    if (resourceClient.StorageAccounts.ListAsync().Result.Count(n => n.Name == name) == 0)
                    {
                        WasDeleted = true;
                    }
                }
                resourceClient.Dispose();
            }
            catch (Exception)
            { return false; }
            return WasDeleted;
        }
        #endregion

        #region Block Blobs
        public async Task<FileUploadSummaryDto> UploadDataToBlob(IFormFile file)
        {
            // Get storage account connection string data from Azure Secrets store
            string connectionString = _dataSummitHelper.GetSecret("datasummitstorage");
            var blobServiceClient = new BlobServiceClient(connectionString);    //v12
            var containerName = Guid.NewGuid().ToString();
            var blobContainerClient = (await blobServiceClient.CreateBlobContainerAsync(containerName)).Value;    //v12

            //Create container if it doesn't exist
            await blobContainerClient.CreateIfNotExistsAsync();

            var identifier = "mysignedidentifier";
            var readWritePermission = "rw";
            var blobSignedIdentifier = new BlobSignedIdentifier()
            {
                Id = identifier,
                AccessPolicy = new BlobAccessPolicy
                {
                    StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                    ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),
                    Permissions = readWritePermission
                }
            };

            var signedIdentifiers = new List<BlobSignedIdentifier>
            {
                blobSignedIdentifier
            };
            _ = await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer, signedIdentifiers);
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(file.FileName);
            var documentFileExtensionEnum = GetDocumentExtensionEnum(file.ContentType);

            // Export image to blockBlob
            var blobUploadOptions = new BlobUploadOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "FileName", HttpUtility.UrlEncode(file.FileName) },
                    { "DocumentFormat", documentFileExtensionEnum.ToString() }
                }
            };

            using (var ms = new MemoryStream())
            {
                var stream = file.OpenReadStream();
                stream.CopyTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                stream.Close();
                await blockBlobClient.UploadAsync(ms, blobUploadOptions);
            }

            var fileName = file.FileName;
            var fileExtension = Path.GetExtension(file.FileName);

            //Add upload data to database
            var doc = new Document()
            {
                //Default seetings to avoid 'NOT NULL' fields from throwing errors
                //TODO enums to replace SQL ids
                DocumentTypeId = 1,         //Unknown type
                ProjectId = 1,              //Empty project
                TemplateVersionId = 1,      //Empty template
                PaperOrientationId = 1,     //Portrait
                PaperSizeId = 9,            //A4

                //Actual document specific data from upload file
                FileName = fileName,
                BlobUrl = blockBlobClient.Uri.ToString(),
                CreatedDate = DateTime.Now,
                ContainerName = containerName,
                FileExtension = fileExtension
            };
            await _documentsDao.CreateDocument(doc);

            return new FileUploadSummaryDto 
            {
                BlobUrl = blockBlobClient.Uri.ToString(),
                FileName = fileName,
                FileExtension = fileExtension
            };
        }

        public BlockBlobClient GetBlobByUrl(string blobUrl)
        {
            // Get storage account connection string data from Azure Secrets store
            string connectionString = _dataSummitHelper.GetSecret("datasummitstorage");
            var blobServiceClient = new BlobServiceClient(connectionString);    //v12

            // Get container from connection Url
            var containerUrl = blobUrl.Substring(0, blobUrl.LastIndexOf("/"));
            var containerName = containerUrl.Substring(containerUrl.LastIndexOf("/") + 1, containerUrl.Length - containerUrl.LastIndexOf("/") - 1);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);    //v12

            var blobName = blobUrl.Substring(blobUrl.LastIndexOf("/") + 1, blobUrl.Length - blobUrl.LastIndexOf("/") - 1);
            var blob = blobContainerClient.GetBlockBlobClient(blobName);

            return blob;
        }
        public async Task AddMetadataToBlob(string blobUrl, List<KeyValuePair<string, string>> metadata)
        {
            var blob = GetBlobByUrl(blobUrl);

            var properties = await blob.GetPropertiesAsync();
            try
            {
                foreach (var pair in metadata)
                {
                    if (properties.Value.Metadata.Count(k => k.Key == pair.Key) > 0)
                    {
                        properties.Value.Metadata.Remove(pair.Key);
                    }
                    properties.Value.Metadata.Add(pair.Key, pair.Value);
                }
                blob.SetMetadata(properties.Value.Metadata);
            }
            catch (Exception)
            {
                throw new Exception("Document type & confidence could not be added to blob metadata");
            }
        }
        public async Task<IDictionary<string, string>> GetBlobMetadata(string blobUrl)
        {
            var blob = GetBlobByUrl(blobUrl);

            var properties = await blob.GetPropertiesAsync();
            return properties.Value.Metadata;
        }
        private DocumentExtension GetDocumentExtensionEnum(string mimeType)
        {
            var format = DocumentExtension.Unknown;

            switch (mimeType)
            {
                case "application/pdf":
                    format = DocumentExtension.PDF;
                    break;
                case "image/jpeg":
                    format = DocumentExtension.JPG;
                    break;
                case "image/x-png":
                    format = DocumentExtension.PNG;
                    break;
                case "image/gif":
                    format = DocumentExtension.GIF;
                    break;
            }

            return format;
        }
        #endregion
    }
}