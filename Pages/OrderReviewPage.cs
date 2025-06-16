using ClientApplication.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ClientApplicationTestProject.Pages
{
    public class OrderReviewPage : BasePage
    {
        public OrderReviewPage(IWebDriver driver) : base(driver) { }
        private By EmailLbl => By.CssSelector("div.user__name label");
        private By CountryTxtField => By.CssSelector("[placeholder*='Country']");
        private By ResultDropdown => By.CssSelector(".ta-results");
        private By CountryDropdownElements => By.CssSelector(".ta-item");

        private By PlaceOrderBtn => By.CssSelector(".action__submit");

        public bool IsIn_OrderReviewPage() 
        {
            return WaitForElementVisible(EmailLbl).Displayed;
        }

        public bool VerifyEmail_IsDisplayed(string userEmail)
        {
            return WaitGetElementText(EmailLbl) == userEmail;
        }

        public void FillCountryInput(string countryName)
        {
            var countryField = WaitForElementVisible(CountryTxtField);
            foreach (char character in countryName)
            {
                countryField.SendKeys(character.ToString());
                System.Threading.Thread.Sleep(100); // Optional: Add a delay to simulate typing speed  
            }
                var dropdownElements = WaitForElementsVisible(CountryDropdownElements);
                foreach (var element in dropdownElements)
                {
                    if (element.Text == countryName)
                    {
                        element.Click();
                    }
                }
            
        }

        public void PlaceOrder() 
        {
            WaitAndClick(PlaceOrderBtn);
        }


    }
}
