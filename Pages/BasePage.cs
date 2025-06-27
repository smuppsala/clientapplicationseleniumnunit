using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Configuration;

namespace ClientApplicationTestProject.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;
        private By OrdersLink => By.CssSelector("button[routerlink*='myorders']");
        private By CartIcon => By.CssSelector("button[routerlink*='cart']");


        // Base constructor for all page objects. Initializes the WebDriver and sets up a WebDriverWait
        protected BasePage(IWebDriver driver)
        {
            Driver = driver;

            int waitTime = 10;
            string configWait = ConfigurationManager.AppSettings["ImplicitWait"];
            if (int.TryParse(configWait, out int result))
            {
                waitTime = result;
            }

            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(waitTime));
        }

        // Wait for element to be visible
        protected IWebElement WaitForElementVisible(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        // Wait for elements to be visible
        protected IReadOnlyCollection<IWebElement> WaitForElementsVisible(By locator)
        {
            return Wait.Until(driver => driver.FindElements(locator));
        }

        // Wait for element to be clickable
        protected IWebElement WaitForElementClickable(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        // Wait and Click
        protected void WaitAndClick(By locator)
        {
            WaitForElementClickable(locator).Click();
        }

        // Wait until element disappears
        protected bool WaitUntilInvisible(By locator)
        {

            return Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        // Wait Clear and Enter Text
        protected void WaitClearAndEnterText(By locator, string text)
        {
            var element = WaitForElementVisible(locator);
            element.Clear();
            element.SendKeys(text);
        }

        // Wait and Get Element Text
        protected string WaitGetElementText(By locator)
        {
            return WaitForElementVisible(locator).Text;
        }

        // Wait until Get the Toasted message text
        protected string GetToastedMessageText(By locator)
        {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator)).Text;
        }



        // Take screenshot with timestamp
        protected void TakeScreenshot(string fileNamePrefix = "Screenshot")
        {
            try
            {
                Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string fileName = $"{fileNamePrefix}_{timestamp}.png";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");

                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                ss.SaveAsFile(Path.Combine(filePath, fileName));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Screenshot failed: {ex.Message}");
            }
        }
    }
}
