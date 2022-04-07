using Karma.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using Karma.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Karma.Data;

namespace KarmaService.Tests
{
    [TestClass]
    public class PictureServiceTests
    {
        PictureService pictureService = new PictureService();
        string photoName = "04b38a45-0140-43f5-b3a0-96d21538a27d_166138862_462283418321277_7954265576154387700_n.jpg";
        string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\", "Karma", "wwwroot"));

        [TestMethod]
        public void DeletePicture_PhotoPathGiven_ReturnsTrue()
        {
            Assert.IsTrue(pictureService.DeletePicture(path, photoName));
        }
        
        [TestMethod]
        public void DeletePicture_PhotoPathIsNull_ReturnsFalse()
        { 
            Assert.IsFalse(pictureService.DeletePicture(path, null));
        }

        [TestMethod]
        public void DeletePicture_RootPathIsNull_ReturnsFalse()
        {
            Assert.IsFalse(pictureService.DeletePicture(null, photoName));
        }

        [TestMethod]
        public void DeletePicture_BothNull_ReturnFalse()
        {
            Assert.IsFalse(pictureService.DeletePicture(null, null));
        }
    }
}
