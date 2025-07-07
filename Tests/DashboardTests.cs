using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Models;
using Microsoft.Testing.Platform.Configurations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientApplicationTestProject.Tests
{
    public class DashboardTests : TestBase
    {
        private LoginPage _loginPage; 
        private DashboardPage _dashboardPage;
        private MainMenuPage _mainMenuPage;

        [SetUp]
        public void BeforeEach()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.GoTo();
            _dashboardPage = _loginPage.LoginWithSecrets();
        }

        [TestCase("ZARA COAT 3")]
        public void ProductCanBeAddedToCart_FromDashboard(string productName)
        {
            _dashboardPage.AddProductToCartByName(productName);
            // Add a wait after adding the product to the cart
            Thread.Sleep(2000); // Wait for 2 seconds
            _mainMenuPage = new MainMenuPage(Driver);
           var items = _mainMenuPage.GetNumberOfProductsInCart();

            Assert.That(items, Is.Not.Null, $"Product '{productName}' has not added to cart.");
        }

        [TestCaseSource(nameof(Product))]
        public void ProductCanBeAddedToCart_FromDashboardUsingProductModel(ProductModel productModel)
        {
            _dashboardPage.AddProductToCartByName(productModel.Product);
            // Add a wait after adding the product to the cart
            Thread.Sleep(2000); // Wait for 2 seconds
            _mainMenuPage = new MainMenuPage(Driver);
            var items = _mainMenuPage.GetNumberOfProductsInCart();

            Assert.That(items, Is.Not.Null, $"Product '{productModel.Product}' has not added to cart.");
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
