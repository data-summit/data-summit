using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureFunctions.DTO;
using DataSummitModels.DTO;
using DataSummitModels.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions
{
    /// <summary>
    /// Divides an image into smaller segments so that they can be processed
    /// Maximum processing size is A4
    /// </summary>
    public static class DivideImage
    {
        private static readonly int MaxPixelSpan = 3000;
        private static readonly string OriginalJpeg = "Original.jpg";

        [FunctionName("DivideImage")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            try
            {
                string bodyContent = await new StreamReader(req.Body).ReadToEndAsync();
                var imageUpload = JsonConvert.DeserializeObject<ImageUploadDto>(bodyContent);

                var imageUploadValidationError = imageUpload.Validate();
                if (!string.IsNullOrEmpty(imageUploadValidationError))
                {
                    return new BadRequestObjectResult(imageUploadValidationError);
                }

                // TODO get storage account connection string data from Azure Secrets store via dependency injection
                string connectionString = $"DefaultEndpointsProtocol=https;AccountName={imageUpload.StorageAccountName};AccountKey={imageUpload.StorageAccountKey};EndpointSuffix=core.windows.net";
                var blobServiceClient = new BlobServiceClient(connectionString);    //v12

                // Get existing container and 'Original' file, which should have been created during the 'Documents.Upload' mothed
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(imageUpload.ContainerName);
                if (!await blobContainerClient.ExistsAsync())
                { return new BadRequestObjectResult($"Illegal input: Cannot find Container: {imageUpload.ContainerName}"); }

                var cloudBlockBlob = blobContainerClient.GetBlobClient(OriginalJpeg);

                using var memoryStream = new MemoryStream();
                _ = await cloudBlockBlob.DownloadToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var image = Image.FromStream(memoryStream);

                using var imageBitmap = image as Bitmap;
                imageBitmap.SetResolution(image.Width, image.Height);

                var imagePortions = ImageToPortions(image.Width, image.Height);
                var splitImages = await CreateImageSections(imageBitmap, imagePortions, blobContainerClient);

                //Write json data file to blob, containing the above information
                var jsonBlob = blobContainerClient.GetBlobClient("Split Images Data & Structure.json");
                var imageUploadJson = JsonConvert.SerializeObject(splitImages, Formatting.Indented);

                using var imageUploadStream = new MemoryStream(Encoding.UTF8.GetBytes(imageUploadJson));
                await jsonBlob.UploadAsync(imageUploadStream);

                return new OkObjectResult(imageUploadJson);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ex));
            }
        }

        private static async Task<string> UploadImageSectionToBlob(BlobContainerClient blobContainerClient, ImageSectionDto splitImage)
        {

            var identifier = "mysignedidentifier";
            var readWritePermission = "rw";

            var signedIdentifiers = new List<BlobSignedIdentifier>
            {
                new BlobSignedIdentifier
                {
                    Id = identifier,
                    AccessPolicy = new BlobAccessPolicy
                    {
                        StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
                        ExpiresOn = DateTimeOffset.UtcNow.AddDays(1),
                        Permissions = readWritePermission
                    }
                }
            };
            await blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer, signedIdentifiers);

            // Export image to blockBlob
            var blobUploadOptions = new BlobUploadOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "Name", splitImage.Name },
                    { "WidthStart", splitImage.WidthStart.ToString()},
                    { "HeightStart", splitImage.HeightStart.ToString()},
                    { "Width", splitImage.Width.ToString() },
                    { "Height", splitImage.Height.ToString()},
                    { "Type", splitImage.ImageType.ToString()}
                }
            };

            using var memStream = new MemoryStream();
            splitImage.Image.Save(memStream, ImageFormat.Jpeg);
            memStream.Seek(0, SeekOrigin.Begin);
            _ = await blobContainerClient.UploadBlobAsync(splitImage.Name, memStream);
            
            var blobClient = blobContainerClient.GetBlobClient(splitImage.Name);
            await blobClient.SetMetadataAsync(blobUploadOptions.Metadata);
            return blobClient.Uri.ToString();
        }

        private static async Task<List<ImageSectionDto>> CreateImageSections(Bitmap bitmap, List<ImagePortionDto> imagePortions, BlobContainerClient blobContainerClient)
        {
            var standardGrid = new List<ImageSectionDto>();

            foreach (var imagePortion in imagePortions)
            {
                var width = imagePortion.AdjustedWith > bitmap.Width ? (bitmap.Width - imagePortion.WidthPixel) : (int)imagePortion.WidthCalculatedSpan;
                var height = imagePortion.AdjustedHeight > bitmap.Height ? bitmap.Height - imagePortion.HeightPixel : (int)imagePortion.HeightCalculatedSpan;
                var imageSection = new ImageSectionDto
                {
                    Name = imagePortion.Name,
                    WidthStart = imagePortion.WidthPixel,
                    HeightStart = imagePortion.HeightPixel,
                    Width = width,
                    Height = height,
                    ImageType = imagePortion.ImageType,
                    Image = bitmap.Clone(new Rectangle(imagePortion.WidthPixel, imagePortion.HeightPixel, width, height), PixelFormat.Format24bppRgb)
                };

                standardGrid.Add(imageSection);
                imageSection.BlobUrl = await UploadImageSectionToBlob(blobContainerClient, imageSection);
            }

            return standardGrid;
        }

        private static List<ImagePortionDto> ImageToPortions(decimal width, decimal height)
        {
            var ImagePortions = new List<ImagePortionDto>();

            // Split width and height based on 'static' MaxPixelSpan
            decimal widthSplitSpan = Math.Ceiling(width / MaxPixelSpan);
            decimal heightSplitSpan = Math.Ceiling(height / MaxPixelSpan);

            // Reverse process to obtain approximately identically sized images
            decimal widthCalculatedSpan = Math.Ceiling(width / widthSplitSpan);
            decimal heightCalculatedSpan = Math.Ceiling(height / heightSplitSpan);

            // Halving span allows for both overlapping & non-overlapping portions
            int widthCalculatedHalfSpan = (int)Math.Round(widthCalculatedSpan / 2, 0);
            int heightCalculatedHalfSpan = (int)Math.Round(heightCalculatedSpan / 2, 0);

            // Max cycles in each direction
            int widthIterations = (int)Math.Round(width / widthCalculatedHalfSpan, 0);
            int heightIterations = (int)Math.Round(height / heightCalculatedHalfSpan, 0);

            // Width positions
            for (int i = 0; i < widthIterations; i++)
            {
                // height positions
                for (int j = 0; j < heightIterations; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0) // Even iteration values
                    {
                        var imagePortion = new ImagePortionDto
                        {
                            HeightCalculatedHalfSpan = heightCalculatedHalfSpan,
                            HeightCalculatedSpan = heightCalculatedSpan,
                            HeightIndex = (int)Math.Floor(j / (decimal)2),
                            HeightIterations = heightIterations,
                            HeightSplit = heightSplitSpan,
                            HeightPixel = (int)Math.Floor(j * (decimal)heightCalculatedHalfSpan),
                            ImageType = ImageType.Normal,
                            WidthCalculatedHalfSpan = widthCalculatedHalfSpan,
                            WidthCalculatedSpan = widthCalculatedSpan,
                            WidthIndex = (int)Math.Floor(i / (decimal)2),
                            WidthIterations = widthIterations,
                            WidthPixel = (int)Math.Floor(i * (decimal)widthCalculatedHalfSpan),
                            WidthSplit = widthSplitSpan,
                            Name = $"{ImageType.Normal}_{Math.Floor(i/(decimal)2):000}-{Math.Floor(j / (decimal)2):000}.jpg",
                        };
                        ImagePortions.Add(imagePortion);
                    }
                    else // Odd iteration values
                    {
                        var imagePortion = new ImagePortionDto
                        {
                            HeightCalculatedHalfSpan = heightCalculatedHalfSpan,
                            HeightCalculatedSpan = heightCalculatedSpan,
                            HeightIndex = (int)Math.Floor(j / (decimal)2),
                            HeightIterations = heightIterations,
                            HeightSplit = heightSplitSpan,
                            HeightPixel = (int)Math.Floor(j * (decimal)heightCalculatedHalfSpan),
                            ImageType = ImageType.Overlap,
                            WidthCalculatedHalfSpan = widthCalculatedHalfSpan,
                            WidthCalculatedSpan = widthCalculatedSpan,
                            WidthIndex = (int)Math.Floor(i / (decimal)2),
                            WidthIterations = widthIterations,
                            WidthPixel = (int)Math.Floor(i * (decimal)widthCalculatedHalfSpan),
                            WidthSplit = widthSplitSpan,
                            Name = $"{ImageType.Overlap}_{Math.Floor(i / (decimal)2):000}-{Math.Floor(j / (decimal)2):000}.jpg",
                        };
                        ImagePortions.Add(imagePortion);
                    }
                }
            }

            return ImagePortions;
        }
    }
}