namespace GLMS.Services
{
    // An observer that sends email notifications when contract status changes
    public class EmailNotification : IObserver
    {
        public string EmailAddress { get; set; } = ""; 

        // Called automatically when the subject (contract) changes
        public void Update(string contractStatus)
        {
            // In a real app, this would actually send an email but right now it just prints to console for testing for part 2 
            Console.WriteLine($"Email sent to {EmailAddress}: Contract status changed to {contractStatus}");
        }
    }
}

// Code attribution
// Title: Observer Design Pattern
// Author: Refactoring Guru
// Date: 2026/04/20
// Version: 1
// Availability: https://refactoring.guru/design-patterns/observer

// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]