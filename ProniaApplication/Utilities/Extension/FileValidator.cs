using ProniaApplication.Models;
using ProniaApplication.Utilities.Enums;
using System.IO;

namespace ProniaApplication.Utilities.Extension
{
    public static class FileValidator
    {
        
        public static bool ValidateType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }

        public static bool ValidateSize(this IFormFile file, FileSize filesize, int size)
        {
            switch (filesize)
            {
                case FileSize.KB: return file.Length <= size * 1024;
                case FileSize.MB: return file.Length <= size * 1024 * 1024;
                case FileSize.GB: return file.Length <= size * 1024 * 1024 * 1024;
            }
            return false;
        }

        public static async Task<string> CreatFileAsync(this IFormFile file, params string[] roots)
        {

            string randomString = Guid.NewGuid().ToString(); 
            
            string extension = file.FileName.Substring(file.FileName.LastIndexOf('.')); 
            
            string fileName = string.Concat(randomString, extension);
            string path = string.Empty;

            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            path = Path.Combine(path, fileName);

            
            using (FileStream fileStream = new(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;

        }

        public static void DeleteFile(this string fileName, params string[] roots)
        {
            string path = string.Empty;

            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }
            path = Path.Combine(path, fileName);

          

            if (File.Exists(path))
            {
                File.Delete(path);
            }
           
        }

       
        
        public static string GenerateFilePath(string[] roots, IFormFile file)
        { 
            string extension = file.FileName.Substring(file.FileName.LastIndexOf('.')); 
            string fileName = string.Concat(Guid.NewGuid().ToString(), extension); 
             string path = string.Empty; 
            for (int i = 0; i < roots.Length; i++) 
            { 
                path = Path.Combine(path, roots[i]); 
            } 
            path = Path.Combine(path, fileName); 
            return path; 
        }

        }
}
