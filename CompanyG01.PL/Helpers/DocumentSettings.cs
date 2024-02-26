using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CompanyG01.PL.Helpers
{
    public static class DocumentSettings
    {
        // Upload
        public static string UploadFile(IFormFile file, string FolderName)
        {
            // 1. Get located folder path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            
            // 2. Get file name and make it unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get file path(FolderPath + FileName)
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. Save file as streams
            using var fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(fs);

            // 5. Return file name
            return FileName;
        }

        // Delete
        public static void DeleteFile(string FileName, string FolderName)
        {
            if (FileName is not null && FolderName is not null)
            {
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName, FileName);
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                }
            }
        }
    }
}
