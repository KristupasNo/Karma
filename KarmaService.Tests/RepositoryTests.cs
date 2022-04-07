using Karma.Data;
using Karma.Models;
using Karma.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarmaService.Tests
{
    [TestClass]
    class RepositoryTests
    {       
        [TestMethod]
        public async Task AddPostAsync_Test()
        {
            var sqlItemRepository = new Mock<SqlItemRepository>();
            var itemPost = new ItemPost();

            sqlItemRepository.Setup( x =>  x.AddPostAsync(itemPost, "labas")).ReturnsAsync(itemPost);
        }
    }
}
