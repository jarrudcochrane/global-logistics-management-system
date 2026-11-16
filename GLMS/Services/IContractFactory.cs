using GLMS.Models;

namespace GLMS.Services
{
    // Interface for creating Contract objects
    public interface IContractFactory
    {
        // Creates a new Contract based on the service level type 
        Contract CreateContract(string type);
    }
}

// Code attribution
// Title: Factory Method Design Pattern
// Author: Refactoring Guru
// Date: 2026/04/20
// Version: 1
// Availability: https://refactoring.guru/design-patterns/factory-method

// Code attribution
// Title: Interfaces in C#
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces

// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]
