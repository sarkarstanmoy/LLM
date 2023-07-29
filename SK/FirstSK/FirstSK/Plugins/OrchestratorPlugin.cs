using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.SkillDefinition;
using System.ComponentModel;

namespace Plugins;

public class OrchestratorPlugin
{
    private IKernel _kernel;

    public OrchestratorPlugin(IKernel kernel)
    {
        _kernel = kernel;
    }

    [SKFunction, Description("Routes the request to the appropriate function.")]
    public async Task<string> RouteRequest(SKContext context)
    {
        // Save the original user request
        string request = context["input"];

        // Add the list of available functions to the context
        context["options"] = "Sqrt, Add";

        // Retrieve the intent from the user request
        var GetIntent = _kernel.Skills.GetFunction("OrchestratorPlugin", "GetIntent");
        await GetIntent.InvokeAsync(context);
        string intent = context["input"].Trim();

        // Call the appropriate function
        switch (intent)
        {
            case "Sqrt":
            // Call the Sqrt function
            case "Add":
            // Call the Add function
            default:
                return "I'm sorry, I don't understand.";
        }
    }
}