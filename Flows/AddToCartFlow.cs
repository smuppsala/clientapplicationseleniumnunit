using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApplicationTestProject.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Flows
{
    public class AddToCartFlow
    {
        private readonly IWebDriver _driver;
        public AddToCartFlow(IWebDriver driver)
        {
            _driver = driver;
        }

        public CartPage AddProductToCartAndGoToCart(string productName) 
        {
            var dashboardPage = new DashboardPage(_driver);
            bool productAdded = dashboardPage.AddProductToCartByName(productName);
            Assert.That(productAdded,Is.True, $"Product '{productName}' was not found on the dashboard.");
            dashboardPage.waitForLoadingToDisappear();
            dashboardPage.GoToCart();

            return new CartPage(_driver);

        }

    }
}
