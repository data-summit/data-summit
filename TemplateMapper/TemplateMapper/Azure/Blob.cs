using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateMapper.OCR.Consolidated;

namespace TemplateMapper.Azure
{
    public static class Blob
    {
        private static string BlobStorageName = "datasummitstorage";
        private static string BlobStorageKey = "UYVxE0qi/CWeDnoyyoqvTVyxbw0X2YuaCSh6CrprGm2VIvussfAU8a2GsKXO8Kz5rADBp3PLSTf+hUj+ZqomBw==";
        public static Image GetImage(string Container)
        {
            Image img = null;
            try
            {
                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + BlobStorageName +
                                           ";AccountKey=" + BlobStorageKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                CloudBlobContainer cbc = blobClient.GetContainerReference(Container);
                CloudBlockBlob blob = null;
                foreach (IListBlobItem blobItem in cbc.ListBlobs(null, true))
                {
                    if (blobItem is CloudBlockBlob)
                    {
                        blob = (CloudBlockBlob)blobItem;
                        if (blob.Name == "Original.jpg") break;
                    }
                }

                MemoryStream ms = new MemoryStream();
                blob.DownloadRangeToStream(ms, 0, blob.Properties.Length);
                ms.Seek(0, SeekOrigin.Begin);
                img = Bitmap.FromStream(ms);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return img;
        }
        public static List<Sentence> GetSentences(string Container)
        {
            List<Sentence> lSentences = new List<Sentence>();
            try
            {
                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + BlobStorageName +
                                           ";AccountKey=" + BlobStorageKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                CloudBlobContainer cbc = blobClient.GetContainerReference(Container);
                CloudBlockBlob blob = null;
                foreach (IListBlobItem blobItem in cbc.ListBlobs(null, true))
                {
                    if (blobItem is CloudBlockBlob)
                    {
                        blob = (CloudBlockBlob)blobItem;
                        if (blob.Name == "All OCR Results.json") break;
                    }
                }

                string sents = blob.DownloadText();
                lSentences = JsonConvert.DeserializeObject<List<Sentence>>(sents);
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return lSentences;
        }
        public static ImageUpload GetImageGrids(string Container)
        {
            ImageUpload imgUp = new ImageUpload();
            try
            {
                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + BlobStorageName +
                                           ";AccountKey=" + BlobStorageKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                CloudBlobContainer cbc = blobClient.GetContainerReference(Container);
                CloudBlockBlob blob = null;
                foreach (IListBlobItem blobItem in cbc.ListBlobs(null, true))
                {
                    if (blobItem is CloudBlockBlob)
                    {
                        blob = (CloudBlockBlob)blobItem;
                        if (blob.Name == "Split Images Data & Structure.json") break;
                    }
                }

                string sents = blob.DownloadText();
                List<ImageGrid> lGrids = JsonConvert.DeserializeObject<List<ImageGrid>>(sents);
                imgUp.SplitImages = lGrids;
                imgUp.ContainerName = Container;
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return imgUp;
        }
        public static ImageUpload GetAllInfo(string Container)
        {
            ImageUpload imgUp = new ImageUpload();
            try
            {
                imgUp.ContainerName = Container;

                string connectionString = @"DefaultEndpointsProtocol=https;AccountName=" + BlobStorageName +
                                           ";AccountKey=" + BlobStorageKey + ";EndpointSuffix=core.windows.net";

                CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = account.CreateCloudBlobClient();
                CloudBlobContainer cbc = blobClient.GetContainerReference(Container);
                CloudBlockBlob blob = null;

                //Get data structure and OCR
                foreach (IListBlobItem blobItem in cbc.ListBlobs(null, true))
                {
                    if (blobItem is CloudBlockBlob)
                    {
                        blob = (CloudBlockBlob)blobItem;
                        if (blob.Name == "Split Images Data & Structure.json")
                        {
                            string sents = blob.DownloadText();
                            List<ImageGrid> lGrids = JsonConvert.DeserializeObject<List<ImageGrid>>(sents);
                            imgUp.SplitImages = lGrids;
                        }
                        if (blobItem is CloudBlockBlob)
                        {
                            blob = (CloudBlockBlob)blobItem;
                            if (blob.Name == "All OCR Results.json")
                            {
                                string sents = blob.DownloadText();
                                imgUp.Sentences = JsonConvert.DeserializeObject<List<Sentence>>(sents);

                            }
                        }
                    }
                }

                ////Get images (asynchronously)
                //List<Task> lAllTasks = new List<Task>();
                //foreach (IListBlobItem blobItem in cbc.ListBlobs(null, true))
                //{
                //    lAllTasks.Add(Task.Run(() =>
                //    {
                //        if (blobItem is CloudBlockBlob)
                //        {
                //            blob = (CloudBlockBlob)blobItem;
                //            if (blob.Name.Substring(blob.Name.Length - 5, 4) == ".jpg")
                //            {
                //                ImageGrid ig = imgUp.SplitImages.FirstOrDefault(g => g.Name == blob.Name);
                //                MemoryStream ms = new MemoryStream();
                //                blob.DownloadRangeToStream(ms, 0, blob.Properties.Length);
                //                ms.Seek(0, SeekOrigin.Begin);
                //                ig.Image = Bitmap.FromStream(ms);
                //            }
                //        }
                //    }));
                //}
                //Task.WaitAll(lAllTasks.ToArray());
            }
            catch (Exception ae)
            {
                string strError = ae.Message.ToString();
            }
            return imgUp;
        }
    }
}
