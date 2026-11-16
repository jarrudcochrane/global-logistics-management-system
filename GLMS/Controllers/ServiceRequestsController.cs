using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IExchangeRateService _exchangeRateService; // Gets USD to ZAR rate
        private readonly CurrencyConverter _currencyConverter; // Converts currency

        public ServiceRequestsController(
            AppDbContext context,
            IExchangeRateService exchangeRateService,
            CurrencyConverter currencyConverter)
        {
            _context = context;
            _exchangeRateService = exchangeRateService;
            _currencyConverter = currencyConverter;
        }

        // Shows all service requests with their related contract and client
        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .Include(s => s.Client)
                .ToListAsync();

            return View(requests);
        }

        // Shows form to create new service request with dropdowns
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View();
        }

        // Saves new service request, validates contract and converts currency
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceRequest request)
        {
            // Check if selected client actually exists
            var clientExists = await _context.Clients
                .AnyAsync(c => c.Id == request.ClientId);

            if (!clientExists)
            {
                ModelState.AddModelError("ClientId", "Please select a valid client.");
            }

            // Find matching contract for this client and service level
            var contract = await _context.Contracts
                .FirstOrDefaultAsync(c =>
                    c.ClientId == request.ClientId &&
                    c.ServiceLevel == request.ContractType);

            if (contract == null)
            {
                ModelState.AddModelError("ContractType", "No contract found for this client and selected type.");
            }
            else if (contract.Status != "Active")
            {
                ModelState.AddModelError("ContractType", $"Cannot create a service request for a contract with status '{contract.Status}'. Only Active contracts are allowed.");
            }

            if (ModelState.IsValid)
            {
                request.ContractId = contract!.Id;

                // Convert USD to ZAR using live exchange rate
                var rate = await _exchangeRateService.GetUsdToZarRateAsync();
                request.CostZAR = _currencyConverter.ConvertCurrency(request.CostUSD, rate);

                _context.ServiceRequests.Add(request);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns(request.ClientId, request.ContractType);
            return View(request);
        }

        // Shows details of one service request
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // Shows delete confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            var request = await _context.ServiceRequests
                .Include(s => s.Contract)
                .Include(s => s.Client)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        // Actually deletes the service request
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request != null)
            {
                _context.ServiceRequests.Remove(request);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method - fills dropdown lists (clients, contract types) and shows exchange rate
        private async Task PopulateDropdowns(
            int? selectedClientId = null,
            string? selectedContractType = null)
        {
            var clients = await _context.Clients
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Clients = new SelectList(clients, "Id", "Name", selectedClientId);

            ViewBag.ContractTypes = new SelectList(
                new List<string> { "Basic", "Standard", "Enterprise" },
                selectedContractType
            );

            ViewBag.ExchangeRate = await _exchangeRateService.GetUsdToZarRateAsync();
        }
    }
}

// Code attribution
// Title: ASP.NET Core MVC Controllers
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/actions

// Code attribution
// Title: Entity Framework Core - Querying Related Data (Include)
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/ef/core/querying/related-data/eager

// Code attribution
// Title: HttpClient - Consume a REST API
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient

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
