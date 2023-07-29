using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.Orchestration;
using System.ComponentModel;

namespace Plugins;

public class MathPlugin
{
    [SKFunction, Description("Takes the square root of a number")]
    public string Sqrt(string number)
    {
        return Math.Sqrt(Convert.ToDouble(number)).ToString();
    }

    [SKFunction, Description("Adds two numbers together")]
    public string Add(SKContext context, string input, string number2)
    {
        return (
            Convert.ToDouble(input) + Convert.ToDouble(number2)
        ).ToString();
    }
}