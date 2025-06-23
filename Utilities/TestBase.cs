using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using ClientApplicationTestProject.Drivers;

namespace ClientApplication.Utilities
{
    public class TestBase
    {
        protected IWebDriver Driver;
        string browser = ConfigurationManager.AppSettings.Get("BaseUrl");

        [SetUp]
        public void Setup()
        {
            Driver = DriverFactory.GetDriver();

            int implicitWait = 5; // Default
            string waitFromConfig = ConfigurationManager.AppSettings["ImplicitWait"];
            if (int.TryParse(waitFromConfig, out int waitConfigValue))
            {
                implicitWait = waitConfigValue;
            }

            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWait);

            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                Driver.Navigate().GoToUrl(baseUrl);
            }


        }

        [TearDown]
        public void Teardown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Console.WriteLine("Test Failed. Capturing screenshot...");
                var screenshotDriver = Driver as ITakesScreenshot;
                if (screenshotDriver != null)
                {
                    var ss = screenshotDriver.GetScreenshot();
                    var fileName = $"FailedTest_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    var filePath = System.IO.Path.Combine(TestContext.CurrentContext.WorkDirectory, fileName);
                    ss.SaveAsFile(filePath);
                    Console.WriteLine($"Screenshot saved to {filePath}");
                }
            }
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
            }
            
        }
    }
}
