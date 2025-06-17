using ClientApplication.Models;
using ClientApplication.Pages;
using ClientApplication.Utilities;
using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Tests
{
    public class OrdersPageTests : TestBase
    {
        private ThankYouOrderPage _thankyouOrderPage;
        private OrdersPage _ordersPage;
        private string productName = "ZARA COAT 3";
        private string countryName = "India";
        private string orderId;

        [SetUp]
        public void SetUp()
        {
            new LoginFlow(Driver).LoginAsValidUser();
            new AddToCartFlow(Driver).AddProductToCartAndGoToCart(productName);
            new CheckoutFlow(Driver).CheckoutAndNavigateToOrderReview();
            _thankyouOrderPage = new PlaceOrderFlow(Driver).PlaceOrderAndNavigateToThankyouPage(countryName);
            _thankyouOrderPage.IsThankYouMessageDisplayed(); // check the page has loaded already 
            
            orderId = _thankyouOrderPage.ExtractOrderId_FromURL();
            _thankyouOrderPage.GoToOrderHistoryPage();

        }
        [Test]
        [Category("smoke")]
        public void Verify_OrderId_IsIn_MyOrdersTable()
        {
            _ordersPage = new OrdersPage(Driver);
            string orderPageHeader = _ordersPage.IsAtOrderPage();
          
            Assert.That(orderPageHeader, Is.EqualTo("Your Orders"));
            string orderIdAtOrderPage = _ordersPage.IsOderIdInOrderList(orderId);
            
            Assert.That(orderIdAtOrderPage, Is.EqualTo(orderId));


        }
    }
}
