namespace GLMS.Services
{
    // Handles PDF file uploads and storage
    public class FileService
    {
        private readonly string _uploadFolder; 

        public FileService(IWebHostEnvironment env)
        {
            
            _uploadFolder = Path.Combine(env.WebRootPath, "uploads");

            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);
        }

        // Saves an uploaded PDF file and returns the unique filename
        public async Task<string> SaveFileAsync(IFormFile file)
        {
            // Check if a file was actually uploaded
            if (file == null || file.Length == 0)
                throw new ArgumentException("No file provided.");

            // Only allow PDF files
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".pdf")
                throw new InvalidOperationException("Only PDF files are allowed.");

            // Create unique filename 
            var uniqueName = Guid.NewGuid().ToString() + ".pdf";
            var filePath = Path.Combine(_uploadFolder, uniqueName);

            // Save the file to disk
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return uniqueName; 
        }

        // Gets full file path for downloading
        public string GetFilePath(string fileName)
        {
            return Path.Combine(_uploadFolder, fileName);
        }
    }
}

// Code attribution
// Title: File uploads in ASP.NET Core
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads

// Code attribution
// Title: IWebHostEnvironment in ASP.NET Core
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment

// Code attribution
// Title: Guid.NewGuid() for unique file naming
// Author: Microsoft
// Date: 2026/04/20
// Version: 1
// Availability: https://learn.microsoft.com/en-us/dotnet/api/system.guid.newguid

// Code attribution
// Title: OpenAI. 2026. ChatGPT (Version 5.3). [Large language model]
// Available at: https://chatgpt.com/share/69e86d41-d970-83ea-926f-3809d5a62e04
// [Accessed: 19 April 2026]