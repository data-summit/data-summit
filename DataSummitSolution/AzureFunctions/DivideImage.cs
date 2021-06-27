using DataSummitModels.DTO;
using DataSummitModels.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureFunctions
{
    /// <summary>
    /// Divides an image into smaller segments so that they can be processed
    /// Maximum processing size is A4
    /// </summary>
    public static class DivideImage
    {
        private static readonly int MaxPixelSpan = 2200;
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

                string connectionString = $"DefaultEndpointsProtocol=https;AccountName={imageUpload.StorageAccountName};AccountKey={imageUpload.StorageAccountKey};EndpointSuffix=core.windows.net";

                var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
                var cloudBlobClient = cloudStorageAccount?.CreateCloudBlobClient();

                var functionTasks = new List<FunctionTaskDto>();
                //if (functionTasks.Count == 0)
                //{ functionTasks.Add(new FunctionTaskDto("Divide Images\tGet container", DateTime.Now)); }
                //else
                //{ functionTasks.Add(new FunctionTaskDto("Divide Images\tGet container", imgUpload.Tasks[functionTasks.Count - 1].TimeStamp)); }

                var cloudBlobContainer = cloudBlobClient.GetContainerReference(imageUpload.ContainerName);
                if (!await cloudBlobContainer.ExistsAsync())
                { return new BadRequestObjectResult($"Illegal input: Cannot find Container: {imageUpload.ContainerName}"); }

                var cloudBlockBlob = await FindOriginalJpeg(cloudBlobContainer, imageUpload.BlobUrl);

                // functionTasks.Add(new FunctionTaskDto("Fetch 'Original.jpg'", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));

                if (cloudBlockBlob == null)
                {
                    return new BadRequestObjectResult($"Could not locate 'Original.jpg' file in container '{imageUpload.ContainerName}'.");
                }

                using var memoryStream = new MemoryStream();
                await cloudBlockBlob.DownloadToStreamAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var image = Image.FromStream(memoryStream);

                decimal pixelWidth = image.Width / MaxPixelSpan;
                var widthMod = (int)Math.Ceiling(pixelWidth);
                var widthSpan = image.Width / widthMod;
                int widthFinalAdjust = image.Width % (widthSpan * widthMod);

                decimal pixelHeight = image.Height / MaxPixelSpan;
                var heightMod = (int)Math.Ceiling(pixelHeight);
                var heightSpan = image.Height / heightMod;
                int heightFinalAdjust = image.Height % (heightSpan * heightMod);

                using var imageBitmap = image as Bitmap;
                imageBitmap.SetResolution(image.Width, image.Height);

                var splitImages = CreateImageSections(widthMod, heightMod, widthSpan, heightSpan, widthFinalAdjust, heightFinalAdjust, imageBitmap, cloudBlobContainer);

                // functionTasks.Add(new FunctionTaskDto("Divide Image\tOriginal divide into " + imageUpload.SplitImages.Count().ToString() + " adjoining images", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));

                var overlappingImages = CreateOverlappingImageSections(widthMod, heightMod, widthSpan, heightSpan, image.Width, image.Height, imageBitmap, cloudBlobContainer);
                splitImages.AddRange(overlappingImages);
                imageUpload.SplitImages = splitImages;

                //functionTasks.Add(new FunctionTaskDto("Divide Image\tOriginal divide into " +
                //                                 imageUpload.SplitImages.Count(d => d.Type == 2).ToString() +
                //                                 " overlapping images", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));

                // functionTasks.Add(new FunctionTaskDto("Divide Image\tCreating JSON Image Structure Text", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));

                //Write json data file to blob, containing the above information
                var jsonBlob = cloudBlobContainer.GetBlockBlobReference("Split Images Data & Structure.json");
                var imageUploadJson = JsonConvert.SerializeObject(imageUpload);
                await jsonBlob.UploadTextAsync(imageUploadJson);

                // functionTasks.Add(new FunctionTaskDto("Divide Image\tFunction complete", imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));
                return new OkObjectResult(imageUploadJson);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ex));
            }
        }

        private static async Task<CloudBlockBlob> FindOriginalJpeg(CloudBlobContainer cloudBlobContainer, string blobUrl)
        {
            var blobResultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(null);
            var blobContinuationToken = blobResultSegment.ContinuationToken;
            var blobSegmentResults = blobResultSegment.Results;
            do
            {
                bool resultsContainBlobUrl = blobSegmentResults.Any(b => b.Uri.ToString() == blobUrl);
                if (resultsContainBlobUrl)
                {
                    var firstMatchingResult = blobSegmentResults.First(b => b.Uri.ToString() == blobUrl);
                    return firstMatchingResult as CloudBlockBlob;
                }
                else
                {
                    foreach (var blobItem in blobSegmentResults)
                    {
                        if (blobItem is CloudBlockBlob)
                        {
                            CloudBlockBlob cloudBlockBlob = blobItem as CloudBlockBlob;

                            if (cloudBlockBlob.Name == OriginalJpeg)
                            {
                                return cloudBlockBlob;
                            }
                        }
                    }

                    return null;
                }
            } while (blobContinuationToken != null);
        }

        private static async void UploadImageSectionToBlob(CloudBlobContainer cloudBlobContainer, ImageSectionDto splitImage)
        {
            var splitImageCloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(splitImage.Name);

            using var memStream = new MemoryStream();
            splitImage.Image.Save(memStream, ImageFormat.Jpeg);
            memStream.Seek(0, SeekOrigin.Begin);
            await splitImageCloudBlockBlob.UploadFromStreamAsync(memStream);

            splitImage.BlobUrl = splitImageCloudBlockBlob.Uri.ToString();

            splitImageCloudBlockBlob.Metadata.Add("Name", splitImage.Name);
            splitImageCloudBlockBlob.Metadata.Add("WidthStart", splitImage.WidthStart.ToString());
            splitImageCloudBlockBlob.Metadata.Add("HeightStart", splitImage.HeightStart.ToString());
            splitImageCloudBlockBlob.Metadata.Add("Width", splitImage.Width.ToString());
            splitImageCloudBlockBlob.Metadata.Add("Height", splitImage.Height.ToString());
            //splitImageCloudBlockBlob.Metadata.Add("Type", splitImage.Type.ToString());
            await splitImageCloudBlockBlob.SetMetadataAsync();

            splitImage.Image = null;

            //functionTasks.Add(new FunctionTaskDto("Divide Image\tImage " +
            //                                    (imageUpload.SplitImages.IndexOf(splitImage) + 1).ToString() + " uploaded",
            //                                    imageUpload.Tasks[functionTasks.Count - 1].TimeStamp));
        }

        private static List<ImageSectionDto> CreateImageSections(int widthMod, 
                                                                     int heightMod, 
                                                                     int widthSpan, 
                                                                     int heightSpan, 
                                                                     int widthAdjust, 
                                                                     int heightAdjust, 
                                                                     Bitmap bitmap,
                                                                     CloudBlobContainer cloudBlobContainer)
        {
            var standardGrid = new List<ImageSectionDto>();

            for (int x = 0; x < widthMod; x++)
            {
                for (int y = 0; y < heightMod; y++)
                {
                    var widthStart = x * widthSpan;
                    var heightStart = y * heightSpan;
                    var imageSection = new ImageSectionDto
                    {
                        Name = $"F_{x:000}-{y:000}.jpg",
                        ImageType = ImageType.Normal,
                        WidthStart = widthStart,
                        HeightStart = heightStart,
                        Width = widthSpan,
                        Height = heightSpan,
                    };

                    if (x == widthMod - 1 && y != heightMod - 1)
                    {
                        imageSection.Width = widthSpan + widthAdjust;
                    }
                    else if (x != widthMod - 1 && y == heightMod - 1)
                    {
                        imageSection.Height = heightSpan + heightAdjust;
                    }
                    else if (x == widthMod - 1 && y == heightMod - 1)
                    {
                        imageSection.Width = widthSpan + widthAdjust;
                        imageSection.Height = heightSpan + heightAdjust;
                    }

                    imageSection.Image = bitmap.Clone(new Rectangle(imageSection.WidthStart, imageSection.HeightStart, imageSection.Width, imageSection.Height), PixelFormat.Format24bppRgb);
                    standardGrid.Add(imageSection);
                    UploadImageSectionToBlob(cloudBlobContainer, imageSection);
                }
            }

            return standardGrid;
        }

        private static List<ImageSectionDto> CreateOverlappingImageSections(int widthMod, 
                                                                           int heightMod, 
                                                                           int widthSpan, 
                                                                           int heightSpan, 
                                                                           int imageWidth, 
                                                                           int imageHeight, 
                                                                           Bitmap bitmap,
                                                                           CloudBlobContainer cloudBlobContainer)
        {
            var overlappingSections = new List<ImageSectionDto>();

            for (int x = 0; x < widthMod; x++)
            {
                for (int y = 0; y < heightMod; y++)
                {
                    var widthStart = (x * widthSpan) + (widthSpan / 2);
                    var heightStart = (y * heightSpan) + (heightSpan / 2);

                    if ((widthStart + widthSpan) < imageWidth && (heightStart + heightSpan) < imageHeight)
                    {
                        var imageGrid = new ImageSectionDto
                        {
                            Name = $"O_{x:000}-{y:000}.jpg",
                            WidthStart = widthStart,
                            HeightStart = heightStart,
                            ImageType = ImageType.Overlap,
                            Width = widthSpan,
                            Height = heightSpan,
                            Image = bitmap.Clone(new Rectangle(widthStart, heightStart, widthSpan, heightSpan), PixelFormat.Format24bppRgb),
                        };

                        overlappingSections.Add(imageGrid);
                        UploadImageSectionToBlob(cloudBlobContainer, imageGrid);
                    }
                }
            }

            return overlappingSections;
        }
    }
}