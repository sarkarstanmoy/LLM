using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.Orchestration;

namespace Plugins;

public class MathPlugin
{
    [SKFunction("Takes the square root of a number")]
    public string Sqrt(string number)
    {
        return Math.Sqrt(Convert.ToDouble(number)).ToString();
    }

    [SKFunction("Adds two numbers together")]
    [SKFunctionContextParameter(Name = "input", Description = "The first number to add")]
    [SKFunctionContextParameter(Name = "number2", Description = "The second number to add")]
    public string Add(SKContext context)
    {
        return (
            Convert.ToDouble(context["input"]) + Convert.ToDouble(context["number2"])
        ).ToString();
    }
}