using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApplicationTestProject.Models;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Flows;
using Microsoft.Testing.Platform.Configurations;
using AventStack.ExtentReports;

namespace ClientApplicationTestProject.Tests
{
    public class OrderReviewPageTests : TestBase
    {
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;
        private CartPage _cartPage;
        private MainMenuPage _mainmenuPage;
        private OrderReviewPage _orderReviewPage;
        private ThankYouOrderPage _thankyouOrderPage;
        private readonly string productName = "ZARA COAT 3";
        private string countryName = "India";

        [SetUp]
        public void SetUp()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.GoTo();
            _dashboardPage = _loginPage.LoginWithSecrets();
            _dashboardPage.AddProductToCartByName(productName); 
            _dashboardPage.waitForLoadingToDisappear();
            _dashboardPage.GoToCart();
            _cartPage = new CartPage(Driver);
        }

        [Test]
        public void ProductIsAvailableAt_OderReviewPage() 
        {
            bool productsAvaliability = _cartPage.ProductsAvailableInCart();
            Assert.That(productsAvaliability, Is.True, $"Products available in the cart.");
            Assert.That(_cartPage.CartItems.Count, Is.EqualTo(1), "Cart should contain one item after adding an item.");

            _orderReviewPage = _cartPage.ProceedToCheckout();
            bool productIsAvailable = _orderReviewPage.ProductsIsAvailableInOrderReview(productName);
            Assert.That(productIsAvailable, Is.True,$"'{productName}' is not available in Order Review Page.");
        }

       [Test]
        public void PlaceOrderSuccessfully_AfterFillingShippingInfo()
        {
            _orderReviewPage.FillCountryInput(countryName);
            _thankyouOrderPage = _orderReviewPage.PlaceOrder();
            bool thankyouTextDisplayed = _thankyouOrderPage.IsThankYouMessageDisplayed();
            Assert.That(thankyouTextDisplayed, Is.True, "Thank you page hasn't displayed after Placing the Order.");
        }
    }
}
