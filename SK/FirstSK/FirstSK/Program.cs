// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Globalization;
using System.Threading.Tasks;
using FirstSK.Plugins;
using LLama.Common;
using LLama;
using LLamaSharp.SemanticKernel.ChatCompletion;
using LLamaSharp.SemanticKernel.TextCompletion;
using LLamaSharp.SemanticKernel.TextEmbedding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.AI.Embeddings;
using Microsoft.SemanticKernel.AI.TextCompletion;
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
string modelPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "huggingface\\gguf\\models\\codellama-7b-instruct.Q2_K.gguf"); // change it to your own model path
var lLamaWeights = LLamaWeights.LoadFromFile(new ModelParams(modelPath));
var context = lLamaWeights.CreateContext(new ModelParams(modelPath, contextSize: 1024, seed: 1337, gpuLayerCount: 1));
// Initialize a chat session
var ex = new InteractiveExecutor(context);

IKernel kernel = Kernel.Builder
    .WithAIService<IChatCompletion>("llama_chat_completion", new LLamaSharpChatCompletion(ex))
    .WithAIService<ITextCompletion>("llama_text_completion", new LLamaSharpTextCompletion(ex)).Build();
    //.WithAIService<ITextEmbeddingGeneration>("llama_text_embedding", new LLamaSharpEmbeddingGeneration(ex)
    //.WithMemoryStorage(memoryStore)
    //.WithChromaMemoryStore(endpoint) // This method offers an alternative approach to registering Chroma memory store.
    

//kernel.Log.LogInformation("this is from logger");

//var mathPlugin = kernel.ImportSkill(new MathPlugin(), "MathPlugin");

//*****Summerization

var pluginsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

var pdfReaderPlugin = kernel.ImportSkill(new PDFReaderPlugin(), "PDFReader");
kernel.ImportSemanticSkillFromDirectory(pluginsDirectory, "SummarizePlugin");

await kernel.RunAsync(pdfReaderPlugin["PDFReader"]);

//******End Summerization



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