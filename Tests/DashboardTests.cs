using ClientApplication.Pages;
using ClientApplication.Utilities;
using ClientApplicationTestProject.Flows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClientApplication.Tests
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
            Assert.IsTrue(productAddedToCart, $"Product '{productName}' was not found on the dashboard.");
        }
    }
}
