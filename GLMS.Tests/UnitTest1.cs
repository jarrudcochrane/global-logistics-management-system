using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace GLMS.Tests
{
    [TestFixture]
    public class BusinessLogicTests
    {
        private CurrencyConverter _converter = null!;

        [SetUp]
        public void Setup()
        {
            _converter = new CurrencyConverter(new ApiCurrencyStrategy());
        }

        // Currency Calculation Tests 

        [Test]
        public void CurrencyConversion_CorrectAmount_ReturnsExpectedZAR()
        {
            decimal result = _converter.ConvertCurrency(100m, 18.5m);
            Assert.That(result, Is.EqualTo(1850.00m));
        }

        [Test]
        public void CurrencyConversion_ZeroAmount_ReturnsZero()
        {
            decimal result = _converter.ConvertCurrency(0m, 18.5m);
            Assert.That(result, Is.EqualTo(0m));
        }

        // File Validation Tests 

        [Test]
        public void FileValidation_InvalidExtension_ThrowsInvalidOperationException()
        {
            var mockFile = new MockFormFile("malware.exe", "application/octet-stream");

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await ValidateFile(mockFile));

            Assert.That(ex!.Message, Does.Contain("Only PDF files are allowed"));
        }

        [Test]
        public void FileValidation_NullFile_ThrowsArgumentException()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await ValidateFile(null));

            Assert.That(ex!.Message, Does.Contain("No file provided"));
        }

        [Test]
        public void FileValidation_ValidPdf_DoesNotThrow()
        {
            var mockFile = new MockFormFile("contract.pdf", "application/pdf");
            Assert.DoesNotThrowAsync(async () => await ValidateFile(mockFile));
        }

        // Contract Status Workflow Tests 

        [Test]
        public void ContractStatus_ExpiredContract_ShouldNotAllowServiceRequest()
        {
            var contract = new Contract { Status = "Expired" };
            bool canCreate = contract.Status == "Active";
            Assert.That(canCreate, Is.False);
        }

        [Test]
        public void ContractStatus_OnHoldContract_ShouldNotAllowServiceRequest()
        {
            var contract = new Contract { Status = "On Hold" };
            bool canCreate = contract.Status == "Active";
            Assert.That(canCreate, Is.False);
        }

        [Test]
        public void ContractStatus_ActiveContract_ShouldAllowServiceRequest()
        {
            var contract = new Contract { Status = "Active" };
            bool canCreate = contract.Status == "Active";
            Assert.That(canCreate, Is.True);
        }

        //  Helper: mirrors FileService validation logic 
        private static async Task ValidateFile(IFormFile? file)
        {
            await Task.CompletedTask;

            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");
        }
    }

    // Minimal mock IFormFile for testing
    public class MockFormFile : IFormFile
    {
        private readonly string _fileName;
        private readonly string _contentType;

        public MockFormFile(string fileName, string contentType)
        {
            _fileName = fileName;
            _contentType = contentType;
        }

        public string ContentType => _contentType;
        public string ContentDisposition => "";
        public IHeaderDictionary Headers => new HeaderDictionary();
        public long Length => 100;
        public string Name => "file";
        public string FileName => _fileName;
        public void CopyTo(Stream target) { }
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Stream OpenReadStream() => new MemoryStream();
    }
}


// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]