using ClientApplicationTestProject.Models;
using ClientApplicationTestProject.Pages;
using OpenQA.Selenium.Support.UI;

namespace ClientApplicationTestProject.Tests
{
    public class DashboardTests : TestBase
    {
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;
        private MainMenuPage _mainMenuPage;
        protected WebDriverWait Wait;

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
            bool productAddedToCart = _dashboardPage.AddProductToCartByName(productName);
            Assert.That(productAddedToCart, Is.True, $"Product '{productName}' was not found on the dashboard.");
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
