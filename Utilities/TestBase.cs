using ClientApplicationTestProject.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ClientApplication.Utilities
{
    public class TestBase
    {
        protected IWebDriver Driver;
        private string _orderId;
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
            var cleaner = new TestDataCleaner(Driver);
            var currentURL = Driver.Url;
            if (currentURL == "https://rahulshettyacademy.com/client/dashboard/myorders")
            {
                cleaner.DeleteTestOrder();
            }
            if (currentURL.Contains ("https://rahulshettyacademy.com/client/dashboard/thanks"))
            {
                cleaner.DeletePlacedOrder();
            }
            if (currentURL == "https://rahulshettyacademy.com/client/dashboard/cart")
            {
                cleaner.ClearCart();
            }
            if (currentURL == "https://rahulshettyacademy.com/client/dashboard/dash")
            {
                cleaner.GoToCartPageIfItemsExist();
                cleaner.ClearCart();
            }
            cleaner.SignOut();

            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
            }

        }
    }
}
