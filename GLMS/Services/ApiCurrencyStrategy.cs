namespace GLMS.Services
{
    // This strategy converts currency using an API exchange rate
    public class ApiCurrencyStrategy : ICurrencyStrategy
    {
        // Converts USD to ZAR by multiplying amount by exchange rate and the Math.Round(.., 2) keeps it to 2 decimal places
        public decimal Convert(decimal amount, decimal rate)
        {
            return Math.Round(amount * rate, 2);
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
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]