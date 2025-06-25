using ClientApplication.Pages;
using ClientApplication.Utilities;
using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Models;
using Microsoft.Testing.Platform.Configurations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientApplicationTestProject.Tests
{
    public class DashboardTests : TestBase
    {
        private DashboardPage _dashboardPage;

        [SetUp]
        public void BeforeEach()
        {
            _dashboardPage = new LoginFlow(Driver).LoginAsValidUser();
        }

        [TestCase("ZARA COAT 3")]
        public void ProductCanBeAddedToCart_FromDashboard(string productName)
        {
            bool productAddedToCart = _dashboardPage.AddProductToCartByName(productName);
            Assert.That(productAddedToCart,Is.True, $"Product '{productName}' was not found on the dashboard.");
        }

        [TestCaseSource(nameof(Product))]
        public void ProductCanBeAddedToCart_FromDashboardUsingProductModel(ProductModel productModel)
        {
            bool productAddedToCart = _dashboardPage.AddProductToCartByName(productModel.Product);
            Assert.That(productAddedToCart, Is.True, $"Product '{productModel.Product}' was not found on the dashboard.");
        }

        public static IEnumerable<ProductModel> Product()
        {
            yield return new ProductModel()
            {
                Product = "ZARA COAT 3"
            };
            yield return new ProductModel()
            {
                Product = "ADIDAS ORIGINAL"
            };

        }
    }
}
