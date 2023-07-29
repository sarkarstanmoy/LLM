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
using System.ComponentModel;

namespace FirstSK.Plugins
{
    public sealed class FRKeyValuePlugin
    {
        [SKFunction, Description("Form recognizer extract key-value data using prebuild model: prebuilt-document")]
        public async Task<string> ExtractDocument(SKContext context, [Description("FR endpoint")] string endpoint,
                                                     [Description("FR API Key")] string apiKey)
        {
            var credential = new AzureKeyCredential(apiKey);
            var client = new DocumentAnalysisClient(new Uri(endpoint), credential);

            // byte[] file = File.ReadAllBytes(fileUri);
            byte[] file = Encoding.ASCII.GetBytes(context["Input"]);
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

            return JsonConvert.SerializeObject(keyValuePairs);
        }
    }

    public class KeyValuePairs
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}