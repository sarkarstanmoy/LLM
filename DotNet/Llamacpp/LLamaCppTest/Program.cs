using LLama.Common;
using LLama;
using Azure.Messaging.WebPubSub;
using static LLama.Common.ChatHistory;

string modelPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "huggingface\\gguf\\models\\codellama-7b-instruct.Q2_K.gguf"); // change it to your own model path
var prompt = "Transcript of a dialog, where the User interacts with an Assistant named Tanistha. Tanistha is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.\r\n\r\nUser: Hello, Tanistha.\r\nTanistha: Hello. How may I help you today?\r\nUser:"; // use the "chat-with-bob" prompt here.

var lLamaWeights = LLamaWeights.LoadFromFile(new ModelParams(modelPath));
var context = lLamaWeights.CreateContext(new ModelParams(modelPath, contextSize: 1024, seed: 1337, gpuLayerCount: 1));
// Initialize a chat session
var ex = new InteractiveExecutor(context);
ChatSession session = new ChatSession(ex);

// show the prompt
Console.WriteLine();
Console.Write(prompt);
var connectionString = "";
var hub = "myhub1";
// run the inference in a loop to chat with LLM
while (prompt != "stop")
{
    foreach (var text in session.Chat(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
    {
        var serviceClient = new WebPubSubServiceClient(connectionString, hub);
        await serviceClient.SendToAllAsync(text);
        Console.Write(text);
    }
    prompt = Console.ReadLine();
}

// save the session
session.SaveSession("SavedSessionPath");
