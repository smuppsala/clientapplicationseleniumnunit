using ClientApplicationTestProject.Models;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Utilities;
using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Pages;
using Microsoft.Testing.Platform.Configurations;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Tests
{
    public class OrdersPageTests : TestBase
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

            _ordersPage = _thankyouOrderPage.GoToOrderHistoryPage();

        }
        [Test]
        [Category("smoke")]
        public void Verify_OrderId_IsIn_MyOrdersTable()
        {
            string orderPageHeader = _ordersPage.IsAtOrderPage();
            Assert.That(orderPageHeader, Is.EqualTo("Your Orders"), "User is not in Orders Page");

            string orderIdAtOrderPage = _ordersPage.CheckAndReturnOderIdFromOrderList(orderId);
            
            Assert.That(orderIdAtOrderPage, Is.EqualTo(orderId), $"Order ID cannot find in Orders Page");


        }
    }
}
