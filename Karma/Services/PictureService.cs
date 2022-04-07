using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Karma.Services
{
    public class PictureService
    {
        public bool DeletePicture(string rootPath, string photoName)
        {
            if (photoName != null && photoName != "noimage.jpg" && rootPath != null)
            {
                string filePath = Path.Combine(rootPath, "images", photoName);
                System.IO.File.Delete(filePath);
                return !System.IO.File.Exists(filePath);
            }
            return false;
        }

        internal string ProcessUploadedFile(string rootPath, IFormFile photo)
        {
            string uniqueFileName = "noimage.jpg";

            if (photo != null)
            {
                string uploadsFolder = Path.Combine(rootPath, "images");

                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                try
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }
                catch
                {
                    System.IO.Directory.CreateDirectory(filePath);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }

            }

            return uniqueFileName;
        }
    }
}
