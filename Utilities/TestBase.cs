using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using ClientApplicationTestProject.Drivers;
using ClientApplicationTestProject.Config;

namespace ClientApplication.Utilities
{
    public class TestBase
    {
        protected IWebDriver Driver;
     //   string browser = ConfigurationManager.AppSettings.Get("BaseUrl");

        [SetUp]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

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
                    var screenshotsDir = System.IO.Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots");
                    if (!System.IO.Directory.Exists(screenshotsDir))
                    {
                        System.IO.Directory.CreateDirectory(screenshotsDir);
                    }
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
