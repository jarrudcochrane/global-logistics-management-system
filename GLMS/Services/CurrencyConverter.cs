namespace GLMS.Services
{
    // Handles currency conversion using different strategies
    public class CurrencyConverter
    {
        private ICurrencyStrategy _strategy; 

        // Constructor - sets the initial strategy
        public CurrencyConverter(ICurrencyStrategy strategy)
        {
            _strategy = strategy;
        }

        // Allows changing the strategy at runtime if needed
        public void SetStrategy(ICurrencyStrategy strategy)
        {
            _strategy = strategy;
        }

        // Performs the conversion using the current strategy
        public decimal ConvertCurrency(decimal amount, decimal rate)
        {
            return _strategy.Convert(amount, rate);
        }
    }
}

// Code attribution
// Title: Strategy Design Pattern
// Author: Refactoring Guru
// Date: 2026/04/20
// Version: 1
// Availability: https://refactoring.guru/design-patterns/strategy

// Code attribution
// Title: Dependency Injection in ASP.NET Core
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection

// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]