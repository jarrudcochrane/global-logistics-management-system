using GLMS.Models;

namespace GLMS.Services
{
    // Creates new Contract objects based on the service level type
    public class ContractFactory : IContractFactory
    {
        // Factory method - returns a pre-configured contract for the given type
        public Contract CreateContract(string type)
        {
            return type switch
            {
                "Enterprise" => new Contract { ServiceLevel = "Enterprise", Status = "Draft" }, 
                "Basic" => new Contract { ServiceLevel = "Basic", Status = "Draft" },         
                _ => new Contract { ServiceLevel = "Standard", Status = "Draft" }             
            };
        }
    }
}
// Code attribution
// Title: Factory Method Design Pattern
// Author: Refactoring Guru
// Date: 2026/04/20
// Version: 1
// Availability: https://refactoring.guru/design-patterns/factory-method

// Code attribution
// Title: C# switch expressions
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/switch-expression


// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]