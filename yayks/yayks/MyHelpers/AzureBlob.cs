using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.Threading.Tasks;
using System.IO;
using static yayks.Models.CommonModels;
using yayks.Models;

namespace yayks.MyHelpers
{
  
    public class AzureBlob
    {
        // Parse the connection string and return a reference to the storage account.
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
     

        public async Task<BlobResultForSaving> UploadImageAsync(NewIMageModel imageToUpload)
        {
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");


            if (await container.CreateIfNotExistsAsync())
            {
                // Enable public access on the newly created "images" container
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess =
                            BlobContainerPublicAccessType.Blob
                    });


            }


            string imageName = imageToUpload.Id + ".png";

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(imageName);
            cloudBlockBlob.Properties.ContentType = imageToUpload.FileExtention;
            await cloudBlockBlob.UploadFromStreamAsync(imageToUpload.File.InputStream);

            BlobResultForSaving res = new BlobResultForSaving()
            {
                URL = cloudBlockBlob.Uri.ToString(),
                BaseUrl = storageAccount.BlobEndpoint.ToString(),
                FileName = imageName


            };
            
            return res;

        }

        public async Task DeleteBlobs(string[] blobFileNames)
        {
            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

            List<string> result = new List<string>();

            foreach (var x in blobFileNames)
            {
                // Retrieve reference to a blob named "myblob.txt".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(x + ".png");

                // Delete the blob.
               await blockBlob.DeleteAsync();
                result.Add(x);

            }
            
        }  



    }

   


}