// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Globalization;
using System.Threading.Tasks;
using FirstSK.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Memory.Chroma;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.Skills.Core;
using Plugins;

string MemoryCollectionName = "chroma-test";

var myLogger = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
}).CreateLogger("Information");

//var memoryStore = new ChromaMemoryStore("http://localhost:8000/");

IKernel kernel = Kernel.Builder
    .WithLogger(myLogger)
.WithAIService
    //.WithAzureTextEmbeddingGenerationService("deployment-text-davinci-003", "https://openai06072023.openai.azure.com/", "afbff89107e24b6a9cddc9e3db81b523")
    .WithAzureTextCompletionService("deployment-text-davinci-003", "https://openai06072023.openai.azure.com/", "afbff89107e24b6a9cddc9e3db81b523")
    //.WithOpenAITextEmbeddingGenerationService("text-embedding-ada-002", "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")
    //.WithMemoryStorage(memoryStore)
    //.WithChromaMemoryStore(endpoint) // This method offers an alternative approach to registering Chroma memory store.
    .Build();

//kernel.Log.LogInformation("this is from logger");

//var mathPlugin = kernel.ImportSkill(new MathPlugin(), "MathPlugin");

//*****Summerization

var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

//var pdfReaderPlugin = kernel.ImportSkill(new PDFReaderPlugin(kernel), "PDFReader");
//kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "SummarizePlugin");

//await kernel.RunAsync(pdfReaderPlugin["PDFReader"]);

//******End Summerization

//**** FormRecognizer

//var FRPlugin = new FRKeyValuePlugin();
//var FRSkill = kernel.ImportSkill(FRPlugin, "FRreader");

PDFReaderPlugin pDFReaderPlugin = new();
var PDFReaderSkill = kernel.ImportSkill(pDFReaderPlugin, "pDFReaderPlugin");
//var HLSPlugin = kernel
//.ImportSemanticSkillFromDirectory(pluginsDirectory, "HLSPlugin");

var variables = new ContextVariables();
//variables.Set("fileUri", Getcurre);
variables.Set("endpoint", "https://hls-dip-dev-fr.cognitiveservices.azure.com/");
variables.Set("apiKey", "5cf0ba909bb140658481e2b77aec54ea");

//var context = await kernel.RunAsync(variables, PDFReaderSkill["PDFReader"], FRSkill["ExtractDocument"]);
//Console.WriteLine(context["Input"]);

var planner = new SequentialPlanner(kernel);
var result = await planner.CreatePlanAsync("Summarize content");
Console.WriteLine("Plan results:");

//****End FormRecognizer

// // Import the OrchestratorPlugin from the plugins directory.
//var orchestratorPlugin = kernel
//     .ImportSemanticSkillFromDirectory(pluginsDirectory, "OrchestratorPlugin");
//var summerizePlugin = kernel
//.ImportSemanticSkillFromDirectory(pluginsDirectory, "SummarizePlugin");

//PDFReaderPlugin pDFReaderPlugin = new();
//var PDFReaderSkill = kernel.ImportSkill(pDFReaderPlugin, "pDFReaderPlugin");
//var context = await kernel.RunAsync(PDFReaderSkill["PDFReader"]);
//Console.WriteLine(context);

// Create a new context and set the input, history, and options variables.
//var context = kernel.CreateNewContext();
//context["input"] = "Yes";
//context["history"] = @"Bot: How can I help you?
// User: My team just hit a major milestone and I would like to send them a message to congratulate them.
// Bot:Would you like to send an email?";
//context["options"] = "SendEmail, ReadEmail, SendMeeting, RsvpToMeeting, SendChat";

//// Get the GetIntent function from the OrchestratorPlugin and run it
//var result = await orchestratorPlugin["GetIntent"].InvokeAsync(context);
//var sresult = await summerizePlugin["Summarize"].InvokeAsync(result.Result);

//Console.WriteLine(result.Result);
//Console.WriteLine(sresult.Result);

//Console.WriteLine("== Printing Collections in DB ==");
//var collections = memoryStore.GetCollectionsAsync();
//await foreach (var collection in collections)
//{
//    Console.WriteLine(collection);
//}

//Console.WriteLine("== Adding Memories ==");

//var key1 = await kernel.Memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: "british short hair");
//var key2 = await kernel.Memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: "orange tabby");
//var key3 = await kernel.Memory.SaveInformationAsync(MemoryCollectionName, id: Guid.NewGuid().ToString(), text: "norwegian forest cat");

//Console.WriteLine("== Printing Collections in DB ==");
//collections = memoryStore.GetCollectionsAsync();
//await foreach (var collection in collections)
//{
//    Console.WriteLine(collection);
//}

//Console.WriteLine("== Retrieving Memories Through the Kernel ==");
//MemoryQueryResult? lookup = await kernel.Memory.GetAsync(MemoryCollectionName, key1);
//Console.WriteLine(lookup != null ? lookup.Metadata.Text : "ERROR: memory not found");

//Console.WriteLine("== Similarity Searching Memories: My favorite color is orange ==");
//var searchResults = kernel.Memory.SearchAsync(MemoryCollectionName, "My favorite color is orange", limit: 3, minRelevanceScore: 0.6);

//await foreach (var item in searchResults)
//{
//    Console.WriteLine(item.Metadata.Text + " : " + item.Relevance);
//}

////Console.WriteLine("== Removing Collection {0} ==", MemoryCollectionName);
////await memoryStore.DeleteCollectionAsync(MemoryCollectionName);

////Console.WriteLine("== Printing Collections in DB ==");
////await foreach (var collection in collections)
////{
////    Console.WriteLine(collection);
////}

//var fileSkill = new MathPlugin();
// var readFileSkill = kernel.ImportSkill(fileSkill, "PDFReader");
// var variables = new ContextVariables();
// variables.Set("path", "C:\\Learnings\\Semantic Kernel\\FirstSK\\FirstSK\\resume.pdf");
// variables.Set("content", "this is from semantic kernel");
// var context = await kernel.RunAsync(variables, readFileSkill["Read"]);

// Console.WriteLine(context);