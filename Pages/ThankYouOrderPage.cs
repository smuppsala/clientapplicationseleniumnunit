using System.Net;
using ClientApplication.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Pages
{
    public class ThankYouOrderPage : BasePage
    {
        public ThankYouOrderPage(IWebDriver driver) : base(driver) { }
        private By ThankyouTextLbl => By.CssSelector(".hero-primary");
        private By OrderRefLbl => By.CssSelector(".em-spacer-1 .ng-star-inserted");
        private By OrderHistoryPageLinkTxt => By.CssSelector("label[routerlink*='myorders']");
        public string _orderId;

        public bool IsThankYouMessageDisplayed()
        {
            return WaitGetElementText(ThankyouTextLbl) == "THANKYOU FOR THE ORDER.";
        }

        public string ExtractOrderId_FromLable()
        {
            string orderIdIncludedTxt = WaitGetElementText(OrderRefLbl);
            string[] refId = orderIdIncludedTxt.Split("| ");
            string orderId = refId[1].Split(" ")[0];
            return orderId;
        }

        public string ExtractOrderId_FromURL()
        {
            string url = Driver.Url;
            string param = url.Split("prop=")[1];
            string decoded = WebUtility.UrlDecode(param); // decoded = ["684c0e8f81a2069530757aad"]
            int firstQuote = decoded.IndexOf('\"') + 1;
            int lastQuote = decoded.LastIndexOf('\"');
            _orderId = decoded.Substring(firstQuote, lastQuote - firstQuote);

            //string orderId = refId[1].Split("%")[0];
            return _orderId;
        }

        public void waitForThankYouPageToDisappear()
        {
            WaitUntilInvisible(ThankyouTextLbl);
        }
        public OrdersPage GoToOrderHistoryPage()
        {
            WaitAndClick(OrderHistoryPageLinkTxt);
            return new OrdersPage(Driver);
        }
    }
}
