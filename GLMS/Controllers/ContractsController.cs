using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    public class ContractsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly FileService _fileService;
        private readonly IContractFactory _contractFactory;

        public ContractsController(AppDbContext context, FileService fileService, IContractFactory contractFactory)
        {
            _context = context;
            _fileService = fileService;
            _contractFactory = contractFactory;
        }

        // Shows all contracts with optional filters (status and date range)
        public async Task<IActionResult> Index(string? status, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(c => c.Status == status);

            if (startDate.HasValue)
                query = query.Where(c => c.StartDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(c => c.EndDate <= endDate.Value);

            ViewBag.StatusFilter = status;
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(await query.ToListAsync());
        }

        // Shows form to create new contract with dropdown list of clients
        public IActionResult Create()
        {
            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        // Saves new contract using ContractFactory handles PDF file upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contract contract, IFormFile? pdfFile)
        {
            // Use the Factory to create the correctly typed contract
            var newContract = _contractFactory.CreateContract(contract.ServiceLevel);
            newContract.ClientId = contract.ClientId;
            newContract.StartDate = contract.StartDate;
            newContract.EndDate = contract.EndDate;
            newContract.Status = contract.Status;

            // If user uploaded a PDF save it to server
            if (pdfFile != null)
            {
                try
                {
                    newContract.SignedAgreementPath = await _fileService.SaveFileAsync(pdfFile);
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name");
                    return View(contract);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(newContract);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clients = new SelectList(_context.Clients, "Id", "Name");
            return View(contract);
        }

        // Downloads the PDF file for a contract
        public IActionResult Download(int id)
        {
            var contract = _context.Contracts.Find(id);
            if (contract == null || string.IsNullOrEmpty(contract.SignedAgreementPath))
                return NotFound();

            var filePath = _fileService.GetFilePath(contract.SignedAgreementPath);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, "application/pdf", contract.SignedAgreementPath);
        }

        // Shows contract details including related service requests
        public async Task<IActionResult> Details(int id)
        {
            var contract = await _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.ServiceRequests)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contract == null) return NotFound();
            return View(contract);
        }

        // Shows delete confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            var contract = await _context.Contracts.Include(c => c.Client)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (contract == null) return NotFound();
            return View(contract);
        }

        // Actually deletes the contract
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract != null) _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
// Title: File uploads in ASP.NET Core
// Author: Microsoft
// Date: 2024
// Version: 1
// Availability: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads

// Code attribution
// Title: Factory Method Design Pattern
// Author: Refactoring Guru
// Date: 2024
// Version: 1
// Availability: https://refactoring.guru/design-patterns/factory-method

// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]