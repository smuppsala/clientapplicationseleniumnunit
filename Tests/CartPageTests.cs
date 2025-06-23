using System;
using NUnit.Framework;
using ClientApplication.Pages;
using ClientApplication.Utilities;
using ClientApplication.Models;
using System.Linq;
using ClientApplicationTestProject.Flows;
using ClientApplicationTestProject.Pages;
using Microsoft.Testing.Platform.Configurations;

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
            _cartPage.IsProductInCart(productName);
            _cartPage.ProceedToCheckout();
            _orderReviewPage = new OrderReviewPage(Driver);
            bool isAtOrderReview = _orderReviewPage.IsIn_OrderReviewPage();
            Assert.That(isAtOrderReview, Is.True);
        }
    }
}
