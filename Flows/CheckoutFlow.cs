using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApplicationTestProject.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Flows
{
    public class CheckoutFlow
    {
        private readonly IWebDriver _driver;
        public CheckoutFlow(IWebDriver driver)
        {
            _driver = driver;
        }

        public OrderReviewPage CheckoutAndNavigateToOrderReview() 
        {
            var cartPage = new CartPage(_driver);
            cartPage.ProceedToCheckout();
            return new OrderReviewPage(_driver);
        }
    }
}
