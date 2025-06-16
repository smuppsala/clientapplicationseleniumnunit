using ClientApplication.Utilities;
using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Pages;

namespace ClientApplicationTestProject.Tests
{
    public class ThankYouOrderPageTests : TestBase
    {
        private ThankYouOrderPage _thankyouOrderPage;
        private OrdersPage _ordersPage;
        private string productName = "ZARA COAT 3";
        private string countryName = "India";

        [SetUp]
        public void SetUp()
        {
            new LoginFlow(Driver).LoginAsValidUser();
            new AddToCartFlow(Driver).AddProductToCartAndGoToCart(productName);
            new CheckoutFlow(Driver).CheckoutAndNavigateToOrderReview();
            _thankyouOrderPage = new PlaceOrderFlow(Driver).PlaceOrderAndNavigateToThankyouPage(countryName);
        }

        [Test]
        public void ThankyouMessage_AndOrderIdHasGenerated()
        {                     
            string orderIdFromLbl = _thankyouOrderPage.ExtractOrderId_FromLable();
            string orderIdFromURL = _thankyouOrderPage.ExtractOrderId_FromURL();
            Assert.IsTrue(_thankyouOrderPage.IsThankYouMessageDisplayed(), "Order confirmation message not displayed.");
            Assert.That(orderIdFromLbl, Is.EqualTo(orderIdFromURL));

        }

        [Test]
        public void NavigateToOrderPage_AfterPlacingOrder()
        {
            _thankyouOrderPage.GoToOrderHistoryPage();
            _ordersPage = new OrdersPage(Driver);
            string orderPageHeading = _ordersPage.IsAtOrderPage();
            Assert.That(orderPageHeading, Is.EqualTo("Your Orders"));
        }
    }
}
