namespace GLMS.Services
{
    // Interface for objects that want to be notified when something changes
    public interface IObserver
    {
        // Called automatically when the subject (contract) changes status
        void Update(string contractStatus);
    }
}

// Code attribution
// Title: Observer Design Pattern
// Author: Refactoring Guru
// Date: 2026/04/20
// Version: 1
// Availability: https://refactoring.guru/design-patterns/observer

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