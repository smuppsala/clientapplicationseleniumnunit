using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Utilities;
using FluentAssert;
using Microsoft.Testing.Platform.Configurations;

namespace ClientApplicationTestProject.Tests
{
    public class ThankYouOrderPageTests : TestBase
    {
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
            new LoginFlow(Driver).LoginAsValidUser();
            new AddToCartFlow(Driver).AddProductToCartAndGoToCart(productName);
            new CheckoutFlow(Driver).CheckoutAndNavigateToOrderReview();
            _thankyouOrderPage = new PlaceOrderFlow(Driver).PlaceOrderAndNavigateToThankyouPage(countryName);
            orderId = _thankyouOrderPage.ExtractOrderId_FromURL();
            // Save the order ID in the cache for access by TestDataCleaner
            TestDataCache.Set(OrderIdCacheKey, orderId);
        }

        [Test, Order(2)]
        public void ThankyouMessage_AndOrderIdHasGenerated()
        {
            string orderIdFromLbl = _thankyouOrderPage.ExtractOrderId_FromLable();
            string orderIdFromURL = _thankyouOrderPage.ExtractOrderId_FromURL();

            Assert.That(_thankyouOrderPage.IsThankYouMessageDisplayed(), Is.True, "Order confirmation message not displayed.");
            Assert.That(orderIdFromLbl, Is.EqualTo(orderIdFromURL));

            // FluentAssertions usage
            orderIdFromLbl.ShouldBeEqualTo(orderIdFromURL); 
        }

        [Test, Order(1)]
        public void NavigateToOrderPage_AfterPlacingOrder()
        {
            _thankyouOrderPage.GoToOrderHistoryPage();
            _ordersPage = new OrdersPage(Driver);
            string orderPageHeading = _ordersPage.IsAtOrderPage();
            Assert.That(orderPageHeading, Is.EqualTo("Your Orders"));

            // FluentAssertions usage
            orderPageHeading.ShouldContain("Your Orders");
        }
    }
}
