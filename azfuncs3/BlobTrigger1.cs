using System;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

namespace Company.Function
{
    public class BlobTrigger1
    {

        [FunctionName("BlobToBlob")]
        public async Task Run([BlobTrigger("sourcecontainer/sourcefolder/{name}", Connection = "AzureWebJobsStorageSrc")]Stream myBlob, string name,  [Blob(
            "destinationcontainer/destinationfolder/{name}",
            FileAccess.Write,
            Connection = "AzureWebJobsStorageDst")]
        Stream outBlob,ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            log.LogInformation($"Starting Copy");
            try{
                await myBlob.CopyToAsync(outBlob);
                log.LogInformation($"Copy completed");
            }
            catch(Exception ex){
                log.LogError(ex.Message);
                log.LogError($"Copy failed");
            }
            finally{
                log.LogInformation($"Operation completed");
            }
        }


        [FunctionName("BlobToAmazonS3")]
        public async Task Run([BlobTrigger("sourcecontainer/sourcefolder/{name}", Connection = "AzureWebJobsStorageSrc")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            await CopyBlob(myBlob, name, log);
            
        }
        public async Task CopyBlob(Stream inBlob, string name, ILogger log)
        {
            var existingBucketName = "yourAmazonS3_bucketname";
            var keyName = name;
            var accessKey = "youraws_iamaccesskey";
            var secretKey = "youraws_iamsecretkey";

            TransferUtility fileTransferUtility = new TransferUtility(new AmazonS3Client(accessKey,secretKey,Amazon.RegionEndpoint.APEast1));

            log.LogInformation($"Starting Copy");

            try{
                await fileTransferUtility.UploadAsync(inBlob,existingBucketName,keyName);
                log.LogInformation($"Copy completed");
            }
            catch(Exception ex){
                log.LogError(ex.Message);
                log.LogError($"Copy failed");
            }
            finally{
                log.LogInformation($"Operation completed");
            }
        }
    }
}
