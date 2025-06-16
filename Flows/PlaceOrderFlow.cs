using ClientApplicationTestProject.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Flows
{
    public class PlaceOrderFlow
    {
        private readonly IWebDriver _driver;
        public PlaceOrderFlow(IWebDriver driver)
        {
            _driver = driver;
        }

        public ThankYouOrderPage PlaceOrderAndNavigateToThankyouPage(string countryName)
        {
            var orderReviewPage = new OrderReviewPage(_driver);
            orderReviewPage.FillCountryInput(countryName);
            orderReviewPage.PlaceOrder();
            return new ThankYouOrderPage(_driver);
        }
    }
}
