using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace Route.C41.G01.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1. Get Located Folder Path
            //string folderPath = $"C:\\Users\\Lenovo\\Desktop\\RoutCourse\\MVC\\Demos\\Route.C41.G01\\Route.C41.G01.PL\\wwwroot\\files\\Images\\{folderName}";
            //string folderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\files\\Images\\{folderName}";
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files" , folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 2. Get FileName and Make It Unique
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // 3. Get File Path
            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save File As Stream

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;

        }

        public static void DeleteFile(string fileName ,  string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot\\files" , folderName , fileName );

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
