using ClientApplication.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Pages
{
    public class OrdersPage : BasePage
    {
        public OrdersPage(IWebDriver driver) : base(driver) { }
        private By YourOrdersLbl => By.CssSelector("h1");
        private By TableRows => By.CssSelector("tbody tr");


        public string IsAtOrderPage()
        {
            WaitForElementsVisible(TableRows);
            return WaitGetElementText(YourOrdersLbl);
        }

        public string IsOderIdInOrderList(string orderId)
        {
           var rows = WaitForElementsVisible(TableRows);
            foreach (var row in rows) 
            {
                var cellOrderId = row.FindElements(By.TagName("th"));
                var oredrIdCount = cellOrderId.Count();
                for (int i = 0; i < oredrIdCount; i++) 
                {
                    string orderIdInRow = cellOrderId.ElementAt(i).Text;
                    if (orderIdInRow == orderId)
                    {
                        return orderIdInRow;
                    }
                }
            }
            return "Unable to Find the OderId in Order List";
        }
    }
}
