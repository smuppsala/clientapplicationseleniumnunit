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
        private string productName = "ZARA COAT 3";
        private string countryName = "India";

        [SetUp]
        public void SetUp()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.GoTo();
            _dashboardPage = _loginPage.LoginWithSecrets();
            _dashboardPage.AddProductToCartByName(productName);
            _dashboardPage.waitForLoadingToDisappear();
            _mainmenuPage = new MainMenuPage(Driver);
            _mainmenuPage.IsItemsExistsInCart();
            _cartPage = _mainmenuPage.GoToCartWhenHaveItemsInIt();
            _cartPage.IsAtCartPage();
            _cartPage.ProceedToCheckout();
            _orderReviewPage = new OrderReviewPage(Driver);
        }

        [Test]
        public void CheckoutProduct_AndReturnToOrderReviewPage()
        {
            _test.Log(Status.Info, "Checking if we're on the Order Review page");
            bool isAtOrderReview = _orderReviewPage.IsIn_OrderReviewPage();

            if (isAtOrderReview)
                _test.Log(Status.Pass, "Successfully navigated to Order Review page");
            else
                _test.Log(Status.Fail, "Failed to navigate to Order Review page");

            Assert.That(isAtOrderReview, Is.True);
        }

        [Test]
        public void ProductIsAvailableAt_OderReviewPage() 
        {
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
