using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Utilities;
using FluentAssert;
using Microsoft.Testing.Platform.Configurations;

namespace ClientApplicationTestProject.Tests
{
    public class ThankYouOrderPageTests : TestBase
    {
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;
        private CartPage _cartPage;
        private OrderReviewPage _orderReviewPage;
        private ThankYouOrderPage _thankyouOrderPage;
        private OrdersPage _ordersPage;
        private string productName = "ZARA COAT 3";
        private string countryName = "India";
        public string orderId;
        // Constants for cache keys
        private const string OrderIdCacheKey = "LastCreatedOrderId";

        [SetUp]
        public void SetUp()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.GoTo();
            _dashboardPage = _loginPage.LoginWithSecrets();
            _dashboardPage.AddProductToCartByName(productName);
            _cartPage = _dashboardPage.GoToCart();
            _orderReviewPage = _cartPage.ProceedToCheckout();
            _orderReviewPage.FillCountryInput(countryName);
            _thankyouOrderPage = _orderReviewPage.PlaceOrder();
            orderId = _thankyouOrderPage.ExtractOrderId_FromURL();
            // Save the order ID in the cache for access by TestDataCleaner
            TestDataCache.Set(OrderIdCacheKey, orderId);
        }

        [Test, Order(1)]
        public void ThankyouMessage_AndOrderIdHasGenerated()
        {
            string orderIdFromLbl = _thankyouOrderPage.ExtractOrderId_FromLable();
            string orderIdFromURL = _thankyouOrderPage.ExtractOrderId_FromURL();

            Assert.That(_thankyouOrderPage.IsThankYouMessageDisplayed(), Is.True, "Order confirmation message not displayed.");
            Assert.That(orderIdFromLbl, Is.EqualTo(orderIdFromURL), $"Order ID displaying on lable '{orderIdFromLbl}' and the Order ID in URL is different '{orderIdFromURL}'");

            // FluentAssertions usage - for learning purpose
            orderIdFromLbl.ShouldBeEqualTo(orderIdFromURL); 
        }

        [Test, Order(2)]
        public void NavigateToOrderPage_FromThankyouPage()
        {
            _ordersPage = _thankyouOrderPage.GoToOrderHistoryPage();
            string orderPageHeading = _ordersPage.IsAtOrderPage();
            Assert.That(orderPageHeading, Is.EqualTo("Your Orders"));

            // FluentAssertions usage
            orderPageHeading.ShouldContain("Your Orders");
        }
    }
}
