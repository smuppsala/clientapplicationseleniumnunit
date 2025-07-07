using ClientApplicationTestProject.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V135.Accessibility;
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
        private By ProductNames => By.ClassName("item__title");
        private By PlaceOrderBtn => By.CssSelector(".action__submit");

        public bool IsIn_OrderReviewPage() 
        {
            return WaitForElementClickable(PlaceOrderBtn).Displayed;
        }

        public bool VerifyEmail_IsDisplayed(string userEmail)
        {
            return WaitGetElementText(EmailLbl) == userEmail;
        }

        public bool ProductsIsAvailableInOrderReview(string productName) 
        {
           var items = WaitForElementsVisible(ProductNames).ToList();
            for (int i = 0; i<= items.Count; i++)
            {

                if (items[i].Text == productName)
                {
                    return true;
                }
            }
            return false;
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

        public ThankYouOrderPage PlaceOrder()
        {
            try
            {
                WaitAndClick(PlaceOrderBtn);
                var thankyouPage = new ThankYouOrderPage(Driver);
                if (!thankyouPage.IsThankYouMessageDisplayed())
                {
                    // Optionally take a screenshot for debugging
                    TakeScreenshot("ThankYouMessageNotDisplayed");
                    throw new Exception("Thank you message was not displayed after placing the order.");
                }
                return thankyouPage;
            }
            catch (Exception ex)
            {
                // Optionally take a screenshot for debugging
                TakeScreenshot("PlaceOrderError");
                throw new Exception("Failed to place the order and Verify thank you message.", ex);
            }
        }


    }
}
