using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApplicationTestProject.Models;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Flows;
using Microsoft.Testing.Platform.Configurations;

namespace ClientApplicationTestProject.Tests
{
    public class OrderReviewPageTests : TestBase
    {
        private OrderReviewPage _orderReviewPage;
        private ThankYouOrderPage _thankyouOrderPage;
        private string productName = "ZARA COAT 3";
        private string countryName = "India";

        [SetUp]
        public void SetUp()
        {
            new LoginFlow(Driver).LoginAsValidUser();
            new AddToCartFlow(Driver).AddProductToCartAndGoToCart(productName);
            _orderReviewPage = new CheckoutFlow(Driver).CheckoutAndNavigateToOrderReview();
        }

        [Test]
        public void PlaceOrderSuccessfully_AfterFillingShippingInfo() 
        {
            _orderReviewPage.FillCountryInput(countryName);
            _orderReviewPage.PlaceOrder();
            _thankyouOrderPage = new ThankYouOrderPage(Driver);
            bool thankyouTextDisplayed = _thankyouOrderPage.IsThankYouMessageDisplayed();
            Assert.That(thankyouTextDisplayed, Is.True, "Order confirmation message not displayed.");
        }
    }
}
