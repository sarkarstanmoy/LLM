using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure;
using Microsoft.SemanticKernel.SkillDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Orchestration;
using Azure.Identity;
using Newtonsoft.Json;
using Azure.Core.Serialization;

namespace FirstSK.Plugins
{
    public class FRPlugin
    {
        [SKFunction("Takes document and extract data using prebuild model")]
        [SKFunctionContextParameter(Name = "endpoint", Description = "FR endpoint")]
        [SKFunctionContextParameter(Name = "fileUri", Description = "File path")]
        [SKFunctionContextParameter(Name = "apiKey", Description = "API Key")]
        public async Task ExtractDocument(SKContext context)
        {
            var credential = new AzureKeyCredential(context["apiKey"]);
            var client = new DocumentAnalysisClient(new Uri(context["endpoint"]), credential);

            byte[] file = File.ReadAllBytes(context["fileUri"]);
            AnalyzeDocumentOperation operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-document", new MemoryStream(file));
            AnalyzeResult result = operation.Value;
            List<KeyValuePairs> keyValuePairs = new List<KeyValuePairs>();

            foreach (var page in result.KeyValuePairs)
            {
                keyValuePairs.Add(new KeyValuePairs()
                {
                    Key = page.Key?.Content,
                    Value = page.Value?.Content
                });
            }

            // context["$input"] = keyValuePairs;
        }
    }

    public class KeyValuePairs
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}