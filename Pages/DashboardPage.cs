﻿using System;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver) : base(driver) { }

        private By ProductList => By.CssSelector(".card-body"); // card of each product
        private By CartIconLink => By.CssSelector("button[routerlink*='cart']");
        private By SuccessMessage => By.CssSelector(".toast-success");
        private By AddToCartSuccessMessage => By.Id("#toast-container");
        private By CartItemsNumber => By.XPath("//button[@class='btn btn-custom']//label");
        private By AddToCartBtns => By.ClassName("fa-shopping-cart");

        public string ProductAddedText() => WaitForElementVisible(SuccessMessage).Text;

        public bool AddProductToCartByName(string productName)
        {
            var products = Driver.FindElements(ProductList);

            foreach (var product in products)
            {
                string productTitle = product.FindElement(By.CssSelector("b")).Text;

                if (productTitle.Equals(productName, StringComparison.OrdinalIgnoreCase))
                {
                    var addToCartButton = product.FindElement(AddToCartBtns);
                    addToCartButton.Click();
                    waitForLoadingToDisappear();
                    return true;
                }
            }

            return false; // Product not found
        }

        public string GetAddedToCartToastedMessage() 
        {
            return GetToastedMessageText(AddToCartSuccessMessage);
        }

        public void waitForLoadingToDisappear() 
        {
            WaitUntilInvisible(AddToCartSuccessMessage);
        }

        public void GoToCart()
        {
            WaitAndClick(CartIconLink);
        }

    }
}
