using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using E_LearnAPI;
using E_LearnAPI.Controllers;

namespace E_LearnAPI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("E-Learning Results Processing", result.ViewBag.Title);
        }
    }
}
