using System;
using NUnit.Framework;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Utilities;
using ClientApplicationTestProject.Models;
using System.Linq;
using ClientApplicationTestProject.Flows;
using Microsoft.Testing.Platform.Configurations;
using AventStack.ExtentReports;

namespace ClientApplicationTestProject.Tests
{
    public class CartPageTests : TestBase
    {
        private CartPage _cartPage;
        private OrderReviewPage _orderReviewPage;
        private readonly string productName = "ZARA COAT 3";

        [SetUp]
        public void SetUp()
        {
            new LoginFlow(Driver).LoginAsValidUser();
            _cartPage = new AddToCartFlow(Driver).AddProductToCartAndGoToCart(productName);
        }

        [Test]
        public void AddedProductDisplayed_InCart()
        {
            bool productInCart = _cartPage.IsProductInCart(productName);
            Assert.That(productInCart,Is.True, $"Product '{productName}' was not found in the cart.");
        }

        [Test]
        public void CheckoutProduct_AndReturnToOrderReviewPage()
        {
            _test.Log(Status.Info, "Verifying product exists in cart");

            bool isProductInCart = _cartPage.IsProductInCart(productName);
           
            if (isProductInCart)
                _test.Log(Status.Pass, $"Product '{productName}' found in cart");
            else
                _test.Log(Status.Fail, $"Product '{productName}' not found in cart");

            _test.Log(Status.Info, "Proceeding to checkout");
            _cartPage.ProceedToCheckout();

            _test.Log(Status.Info, "Checking if we're on the Order Review page");
            _orderReviewPage = new OrderReviewPage(Driver);
            bool isAtOrderReview = _orderReviewPage.IsIn_OrderReviewPage();

            if (isAtOrderReview)
                _test.Log(Status.Pass, "Successfully navigated to Order Review page");
            else
                _test.Log(Status.Fail, "Failed to navigate to Order Review page");

            Assert.That(isAtOrderReview, Is.True);
        }
    }
}
