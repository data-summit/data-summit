using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
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
using System.Linq;
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
                var data = await cloudBlockBlob.DownloadToAsync(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var image = Image.FromStream(memoryStream);

                using var imageBitmap = image as Bitmap;
                imageBitmap.SetResolution(image.Width, image.Height);

                var imagePortions = ImageToPortions(image.Width, image.Height);
                var splitImages = await CreateImageSections(imageBitmap, imagePortions, blobContainerClient);

                //Write json data file to blob, containing the above information
                var jsonBlob = blobContainerClient.GetBlobClient("Split Images Data & Structure.json");
                var imageUploadJson = JsonConvert.SerializeObject(splitImages, Formatting.Indented);

                using var ms = new MemoryStream(Encoding.UTF8.GetBytes(imageUploadJson));
                await jsonBlob.UploadAsync(ms);

                return new OkObjectResult(imageUploadJson);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(JsonConvert.SerializeObject(ex));
            }
        }

        private static async Task<BlobItem> FindOriginalJpeg(BlobContainerClient blobContainerClient, string blobUrl)
        {
            // v12
            var blobsAsPages = blobContainerClient.GetBlobsAsync().AsPages();
            await foreach (var blobPage in blobsAsPages)
            {
                foreach (var blobItem in blobPage.Values)
                {

                    if (blobItem.Name == OriginalJpeg)
                    { return blobItem; }
                }
            }
            return null;
            //// v11
            //var blobResultSegment = await blobContainerClient.ListBlobsSegmentedAsync(null);
            //var blobContinuationToken = blobResultSegment.ContinuationToken;
            //var blobSegmentResults = blobResultSegment.Results;
            //do
            //{
            //    bool resultsContainBlobUrl = blobSegmentResults.Any(b => b.Uri.ToString() == blobUrl);
            //    if (resultsContainBlobUrl)
            //    {
            //        var firstMatchingResult = blobSegmentResults.First(b => b.Uri.ToString() == blobUrl);
            //        return firstMatchingResult as CloudBlockBlob;
            //    }
            //    else
            //    {
            //        foreach (var blobItem in blobSegmentResults)
            //        {
            //            if (blobItem is CloudBlockBlob)
            //            {
            //                CloudBlockBlob cloudBlockBlob = blobItem as CloudBlockBlob;

            //                if (cloudBlockBlob.Name == OriginalJpeg)
            //                {
            //                    return cloudBlockBlob;
            //                }
            //            }
            //        }

            //        return null;
            //    }
            //} while (blobContinuationToken != null);
        }

        private static async Task<string> UploadImageSectionToBlob(BlobContainerClient blobContainerClient,
                                                           ImageSectionDto splitImage)
        {

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
            { blobSignedIdentifier };
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
            var blobContainerInfo = await blobContainerClient.UploadBlobAsync(splitImage.Name, memStream);
            var blobClient = blobContainerClient.GetBlobClient(splitImage.Name);

            await blobClient.SetMetadataAsync(blobUploadOptions.Metadata);
            return blobClient.Uri.ToString();
        }

        private static async Task<List<ImageSectionDto>> CreateImageSections(Bitmap bitmap, List<ImagePortion> imagePortions,
                                                                     BlobContainerClient blobContainerClient)
        {
            var standardGrid = new List<ImageSectionDto>();

            foreach (var imagePortion in imagePortions)
            {
                var imageSection = new ImageSectionDto
                {
                    Name = imagePortion.Name,
                    WidthStart = imagePortion.WidthPixel,
                    HeightStart = imagePortion.HeightPixel,
                    Width = (int)imagePortion.WidthCalculatedSpan,
                    Height = (int)imagePortion.HeightCalculatedSpan,
                };

                // Assign type from prefix
                if (imagePortion.Prefix == 'F')
                { imageSection.ImageType = ImageType.Normal; }
                else
                { imageSection.ImageType = ImageType.Overlap; }

                // Check that final pixel is within range of bitmap dimensions
                if ((imageSection.WidthStart + imageSection.Width) > bitmap.Width)
                { imageSection.Width = bitmap.Width - imageSection.WidthStart; }
                if ((imageSection.HeightStart + imageSection.Height) > bitmap.Height)
                { imageSection.Height = bitmap.Height - imageSection.HeightStart; }

                imageSection.Image = bitmap.Clone(new Rectangle(imageSection.WidthStart, imageSection.HeightStart, imageSection.Width, imageSection.Height), PixelFormat.Format24bppRgb);
                standardGrid.Add(imageSection);
                imageSection.BlobUrl = await UploadImageSectionToBlob(blobContainerClient, imageSection);
            }

            return standardGrid;
        }

        private static async Task<List<ImageSectionDto>> CreateImageSections(int widthMod,
                                                                     int heightMod,
                                                                     int widthSpan,
                                                                     int heightSpan,
                                                                     int widthAdjust,
                                                                     int heightAdjust,
                                                                     Bitmap bitmap,
                                                                     BlobServiceClient blobServiceClient,
                                                                     BlobContainerClient blobContainerClient)
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
                    await UploadImageSectionToBlob(blobContainerClient, imageSection);
                }
            }

            return standardGrid;
        }

        private static async Task<List<ImageSectionDto>> CreateOverlappingImageSections(int widthMod,
                                                                           int heightMod,
                                                                           int widthSpan,
                                                                           int heightSpan,
                                                                           int imageWidth,
                                                                           int imageHeight,
                                                                           Bitmap bitmap,
                                                                           BlobServiceClient blobServiceClient,
                                                                           BlobContainerClient blobContainerClient)
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
                        await UploadImageSectionToBlob(blobContainerClient, imageGrid);
                    }
                }
            }

            return overlappingSections;
        }

        private static List<ImagePortion> ImageToPortions(int width, int height)
        {
            var ImagePortions = new List<ImagePortion>();

            // Split width and height based on 'static' MaxPixelSpan
            decimal widthSplitSpan = Math.Ceiling(width / (decimal)MaxPixelSpan);
            decimal heightSplitSpan = Math.Ceiling(height / (decimal)MaxPixelSpan);

            // Reverse process to obtain approximately identically sized images
            decimal widthCalculatedSpan = Math.Ceiling(width / widthSplitSpan);
            decimal heightCalculatedSpan = Math.Ceiling(height / heightSplitSpan);

            // Halving span allows for both overlapping & non-overlapping portions
            int widthCalculatedHalfSpan = (int)Math.Round(widthCalculatedSpan / 2, 0);
            int heightCalculatedHalfSpan = (int)Math.Round(heightCalculatedSpan / 2, 0);

            // Max cycles in each direction
            int widthIterations = (int)Math.Round(width / (decimal)widthCalculatedHalfSpan, 0);
            int heightIterations = (int)Math.Round(height / (decimal)heightCalculatedHalfSpan, 0);

            // Width positions
            for (int i = 0; i < widthIterations; i++)
            {
                // height positions
                for (int j = 0; j < heightIterations; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0) // Even iteration values
                    {
                        var imagePortion = new ImagePortion
                        {
                            HeightCalculatedHalfSpan = heightCalculatedHalfSpan,
                            HeightCalculatedSpan = heightCalculatedSpan,
                            HeightIndex = (int)Math.Floor(j / (decimal)2),
                            HeightIterations = heightIterations,
                            HeightSplit = heightSplitSpan,
                            HeightPixel = (int)Math.Floor(j * (decimal)heightCalculatedHalfSpan),
                            Prefix = 'F',
                            WidthCalculatedHalfSpan = widthCalculatedHalfSpan,
                            WidthCalculatedSpan = widthCalculatedSpan,
                            WidthIndex = (int)Math.Floor(i / (decimal)2),
                            WidthIterations = widthIterations,
                            WidthPixel = (int)Math.Floor(i * (decimal)widthCalculatedHalfSpan),
                            WidthSplit = widthSplitSpan
                        };
                        imagePortion.Name = $"{imagePortion.Prefix}_{Math.Floor(i/(decimal)2):000}-{Math.Floor(j / (decimal)2):000}.jpg";
                        ImagePortions.Add(imagePortion);
                    }
                    if (i % 2 == 1 && j % 2 == 1) // Odd iteration values
                    {

                        var imagePortion = new ImagePortion
                        {
                            HeightCalculatedHalfSpan = heightCalculatedHalfSpan,
                            HeightCalculatedSpan = heightCalculatedSpan,
                            HeightIndex = (int)Math.Floor(j / (decimal)2),
                            HeightIterations = heightIterations,
                            HeightSplit = heightSplitSpan,
                            HeightPixel = (int)Math.Floor(j * (decimal)heightCalculatedHalfSpan),
                            Prefix = 'O',
                            WidthCalculatedHalfSpan = widthCalculatedHalfSpan,
                            WidthCalculatedSpan = widthCalculatedSpan,
                            WidthIndex = (int)Math.Floor(i / (decimal)2),
                            WidthIterations = widthIterations,
                            WidthPixel = (int)Math.Floor(i * (decimal)widthCalculatedHalfSpan),
                            WidthSplit = widthSplitSpan
                        };
                        imagePortion.Name = $"{imagePortion.Prefix}_{Math.Floor(i / (decimal)2):000}-{Math.Floor(j / (decimal)2):000}.jpg";
                        ImagePortions.Add(imagePortion);

                    }
                }
            }

            ////Image coordinate verification outputs
            //ImagePortions = ImagePortions.OrderBy(r => r.Name).ToList();
            //Console.WriteLine("Type\tW Pix\tHPix\tW Ind\tH In\tName");
            //ImagePortions.ForEach(ip => Console.WriteLine(ip.Prefix + "\t" + ip.WidthPixel + "\t" + ip.HeightPixel + "\t" + ip.WidthIndex + "\t" + ip.HeightIndex + "\t" + ip.Name));

            return ImagePortions;
        }
    }


    class ImagePortion
    {
        public char Prefix { get; set; }
        public decimal WidthSplit { get; set; }
        public decimal HeightSplit { get; set; }
        public decimal WidthCalculatedSpan { get; set; }
        public decimal HeightCalculatedSpan { get; set; }
        public decimal WidthCalculatedHalfSpan { get; set; }
        public decimal HeightCalculatedHalfSpan { get; set; }
        public int WidthIterations { get; set; }
        public int HeightIterations { get; set; }
        public int WidthPixel { get; set; }
        public int HeightPixel { get; set; }
        public int WidthIndex { get; set; }
        public int HeightIndex { get; set; }
        public string Name { get; set; }
    }
}