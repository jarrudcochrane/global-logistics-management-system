using Newtonsoft.Json.Linq;

namespace GLMS.Services
{
    // Fetches live USD to ZAR exchange rates from an external Public and free API
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl; 

        public ExchangeRateService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUrl = configuration["ExchangeRateApi:BaseUrl"]
                      ?? "https://open.er-api.com/v6/latest/USD";
        }

        // Gets current exchange rate 
        public async Task<decimal> GetUsdToZarRateAsync()
        {
            try
            {
                // Call the API and get JSON response
                var response = await _httpClient.GetStringAsync(_apiUrl);
                var json = JObject.Parse(response);

                // Extract ZAR rate from JSON, fallback to 18.5 if missing
                var rate = json["rates"]?["ZAR"]?.Value<decimal>() ?? 18.5m;
                return rate;
            }
            catch
            {
                return 18.5m;
            }
        }
    }
}

// Code attribution
// Title: Exchange Rate API - Free Open Currency Exchange Rates API
// Author: Exchange Rate API
// Date: 2026/04/20
// Version: 1
// Availability: https://www.exchangerate-api.com/docs/free

// Code attribution
// Title: Open Exchange Rates API Endpoint
// Author: Exchange Rate API
// Date: 2026/04/20
// Version: 1
// Availability: https://open.er-api.com/v6/latest/USD

// Code attribution
// Title: HttpClient - Consume a REST API
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient

// Code attribution
// Title: IConfiguration in ASP.NET Core
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration

// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]