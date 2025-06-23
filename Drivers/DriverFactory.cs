using System.Configuration;
using System.Collections.Specialized;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace ClientApplicationTestProject.Drivers
{
    public class DriverFactory
    {
        

        public static IWebDriver GetDriver()
        {
            string browser = ConfigurationManager.AppSettings.Get("Browser");

           /* if (string.IsNullOrEmpty(browser))
            {
                throw new ConfigurationErrorsException("The 'Browser' setting is missing or empty in the configuration file.");
            } */

            IWebDriver driver;

            switch (browser.ToLower())
            {
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "chrome":
                default:
                    driver = new ChromeDriver();
                    break;
            }

            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
