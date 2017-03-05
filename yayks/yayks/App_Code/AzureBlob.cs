using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
using System.Threading.Tasks;
using System.IO;

namespace yayks.Helpers
{
    public class BlobResultForSaving
    {
        public string BaseUrl { get; set; }
        public string FileName { get; set; }
        public string URL { get; set; }

    }
    public class AzureBlob
    {
        // Parse the connection string and return a reference to the storage account.
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

        public async Task<BlobResultForSaving> UploadImageAsync(HttpPostedFileBase imageToUpload)
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
            

            string imageName = Guid.NewGuid().ToString() + "-" + Path.GetExtension(imageToUpload.FileName);

            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(imageName);
            cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;
            await cloudBlockBlob.UploadFromStreamAsync(imageToUpload.InputStream);

            BlobResultForSaving res = new BlobResultForSaving()
            {
                URL = cloudBlockBlob.Uri.ToString(),
                BaseUrl = storageAccount.BlobEndpoint.ToString(),
                FileName = imageName,


            };
            
            return res;

        }


    }

   


}