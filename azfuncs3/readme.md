## What it does?

- Leverage the blob trigger capability to copy one blob from Azure Storage Account to
 - another Azure Storage Account
 - Amazon S3 bucket

## Things to be prepared

- Azure Storage Account any tiers with Azure Queue. Pay attention to the ADLS2 (hierarchical namespace enabled) never incorporate the Azure Queue.
- Function app with any billing models (Consumption / Elastic Premium / App service plan)
- Amazon S3 bucket with IAM user granted with bucket list and put permission like *AmazonS3FullAccess* as well as the access and secret key.
- Vistual Studio Code with the following extensions installed
 - [Azure Tools extension pack](https://marketplace.visualstudio.com/items?itemName=ms-vscode.vscode-node-azure-pack)
 - [Azure Functions](https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azurefunctions)



## Install libraries and frameworks
- Open the project folder via VSC, run the following commands in Terminal.
```javascript
npm install -g azure-functions-core-tools@4 --unsafe-perm true  
dotnet add package Microsoft.Azure.WebJobs.Extensions.Storage.Blobs
dotnet add package AWSSDK.S3
```

## Replace storage account and  bucket configuration
#####local.settings.json
    "AzureWebJobsStorageSrc": "DefaultEndpointsProtocol=https;AccountName={yoursrcstorageacctname};AccountKey={yoursrcstorageacctkey};EndpointSuffix=core.windows.net",
    "AzureWebJobsStorageDst": "DefaultEndpointsProtocol=https;AccountName={yourdststorageacctname};AccountKey={yourdststorageacctkey};EndpointSuffix=core.windows.net",
#####BlobTrigger1.cs
            var existingBucketName = "yourAmazonS3_bucketname";
            var accessKey = "youraws_iamaccesskey";
            var secretKey = "youraws_iamsecretkey";

## Test
- Press F5 to debug
######You will be asked to sign in with your Azure account and select your provisioned function app
- Select function **BlobToBlob** for Azure to Azure
- Select function **BlobToAmazonS3** for Azure to Azure
- Upload any files to your source container to trigger the function works.

## Deploy
- Click deploy. You can find it in the Azure function extension > workspace
[Read reference here](https://learn.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?source=recommendations&tabs=csharp#republish-project-files)

##Credit
- Since the original source code from [stackoverflow](https://stackoverflow.com/questions/46861723/copy-from-azure-blob-to-aws-s3-using-c-sharp) is too outdated. Therefore, I revised the code with 2022 SDK and blob libraries.

Hope this helpful, cheer to **OpenSource Century**!
