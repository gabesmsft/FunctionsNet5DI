using Microsoft.Azure.Functions.Worker;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json.Linq;

namespace FunctionsNet5DI
{
    public static class BlobTriggerGetMetaData
    {
        [Function("BlobTriggerGetMetaData")]
        public static MyOutputType Run(
            [BlobTrigger("test-samples1/{name}", Connection = "AzureWebJobsStorage")] string myBlob,
            FunctionContext context)
        {
            var logger = context.GetLogger("BlobTriggerGetMetaData");

            /*
            // An unelegant proof-of-concept example for getting metadata property values from a Blob trigger.
            // I used the following commented code to determine the name for each property, which may differ from the doc.

            var bindingMetaDataKeys = context.BindingContext.BindingData.Keys;
            foreach (string s in bindingMetaDataKeys)
            {
                logger.LogInformation("Key name: " + s);
            }
            */

            var blobUri = context.BindingContext.BindingData.GetValueOrDefault("Uri").ToString();

            logger.LogInformation("Uri metadata grabbed via my code: " + blobUri);


            var blobMetaDataPropertiesJson = context.BindingContext.BindingData.GetValueOrDefault("Properties").ToString();
            
            //logger.LogInformation("Blob properties metadata dump: " + blobMetaDataPropertiesJson);

            JObject jsonObject = JObject.Parse(@blobMetaDataPropertiesJson.ToString());

            string LastModified = (string)jsonObject["LastModified"];
            logger.LogInformation("LastModified metadata grabbed via my code: " + LastModified);

            //An example of doing multiple output bindings. In this case, we are outputting the trigger blob's blobUri metadata to a Queue message, and also outputting the trigger blob to a destination container.

            return new MyOutputType()
            {
                QueueOutput = blobUri,
                BlobOutput = myBlob
            };
        }
    }
    public class MyOutputType
    {
        [QueueOutput("destinationqueue", Connection = "AzureWebJobsStorage")]
        public string QueueOutput { get; set; }

        [BlobOutput("destinationcontainer/{name}", Connection = "AzureWebJobsStorage")]
        public string BlobOutput { get; set; }
    }

}
