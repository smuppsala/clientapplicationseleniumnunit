using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ClientApplicationTestProject.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;
        private static readonly IConfiguration configuration;
        private By OrdersLink => By.CssSelector("button[routerlink*='myorders']");
        private By CartIcon => By.CssSelector("button[routerlink*='cart']");

        //static constructor to initialize configuration
        static BasePage()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
        // Base constructor for all page objects. Initializes the WebDriver and sets up a WebDriverWait
        protected BasePage(IWebDriver driver)
        {

            Driver = driver;

            int waitTime = 10; //default wait time
            string? configWaitSetting = configuration["TestSettings:ImplicitWait"];
            if (int.TryParse(configWaitSetting, out int result))
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
            try
            {
                // First ensure element is visible
                WaitForElementVisible(locator);

                // Then attempt standard click on clickable element
                WaitForElementClickable(locator).Click();
            }
            catch (ElementClickInterceptedException)
            {
                // If click is intercepted, try JavaScript approach
                IWebElement element = Driver.FindElement(locator);

                // Scroll element into view first
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);

                // Small delay after scrolling
                Thread.Sleep(300);

                try
                {
                    element.Click();
                }
                catch (Exception)
                {
                    // Force click using JavaScript as last resort
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
                }
            }
        }
        // wait element to be clickable and click by index
        protected void WaitForElementsToBeClickableAndClickByIndex(IReadOnlyCollection<IWebElement> elements, int index = 0)
        {
            // First check if collection has elements
            Wait.Until(driver => elements.Count > 0);

            // Wait until all elements in collection are both displayed and enabled
            Wait.Until(driver => elements.ElementAtOrDefault(index) != null &&
                                 elements.ElementAt(index).Displayed &&
                                 elements.ElementAt(index).Enabled);

            // Click the element at the specified index (default is first element)
            elements.ElementAt(index).Click();
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
        protected string WaitForToastAndGetText(By locator)
        {
            try
            {
                // Use a short explicit wait focused on toast messages
                var toastWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(3));
                toastWait.PollingInterval = TimeSpan.FromMilliseconds(100);

                // Wait for the element to be visible and get its text in one operation
                string text = toastWait.Until(driver =>
                {
                    try
                    {
                        var element = driver.FindElement(locator);
                        return element.Displayed ? element.Text : null;
                    }
                    catch
                    {
                        return null;
                    }
                });

                return text ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
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
