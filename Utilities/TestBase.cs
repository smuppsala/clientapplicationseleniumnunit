using AventStack.ExtentReports;
using ClientApplicationTestProject.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ClientApplication.Utilities
{
    public class TestBase
    {
        protected IWebDriver Driver;
        protected ExtentTest _test;
        //   string browser = ConfigurationManager.AppSettings.Get("BaseUrl");

        [OneTimeSetUp]
        public void BeforeClass()
        {
            //Initialize Extent Report at the begining of the test class execution
            ExtentReportHelper.InitializeReport();
        }

        [SetUp]
        public void Setup()
        {
            //Create a new test in the report for each test case
            _test = ExtentReportHelper.Extent.CreateTest(TestContext.CurrentContext.Test.Name,
                "Test Description: " + TestContext.CurrentContext.Test.Properties.Get("Description"));

            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _test.Log(Status.Info, "Browser started and configured");

        }

        [TearDown]
        public void Teardown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var errorMessage = string.IsNullOrEmpty(TestContext.CurrentContext.Result.Message)
                ? "" : TestContext.CurrentContext.Result.Message;
            var stackTrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace)
                ? "" : TestContext.CurrentContext.Result.StackTrace;

            switch (status)
            {
                case NUnit.Framework.Interfaces.TestStatus.Failed:
                    _test.Log(Status.Fail, $"Test failed with error: {errorMessage}");
                    _test.Log(Status.Fail, $"Stack Trace: {stackTrace}");

                    // Capture screenshot for failed test
                    if (Driver != null)
                    {
                        string screenshotPath = ExtentReportHelper.CaptureScreenshot(Driver, TestContext.CurrentContext.Test.Name);
                        _test.AddScreenCaptureFromPath(screenshotPath, "Screenshot of failure");
                    }
                    break;

                case NUnit.Framework.Interfaces.TestStatus.Passed:
                    _test.Log(Status.Pass, "Test passed successfully");
                    break;

                case NUnit.Framework.Interfaces.TestStatus.Skipped:
                    _test.Log(Status.Skip, "Test skipped");
                    break;

                default:
                    _test.Log(Status.Warning, "Test completed with unknown status");
                    break;
            }

            // Cleanup code
            var cleaner = new TestDataCleaner(Driver);
            var currentURL = Driver.Url;
            if (currentURL == "https://rahulshettyacademy.com/client/dashboard/myorders")
            {
                cleaner.DeleteTestOrder();
            }
            if (currentURL.Contains("https://rahulshettyacademy.com/client/dashboard/thanks"))
            {
                cleaner.DeletePlacedOrder();
            }
            if (currentURL == "https://rahulshettyacademy.com/client/dashboard/cart")
            {
                cleaner.ClearCart();
            }
            if (currentURL.Contains("https://rahulshettyacademy.com/client/dashboard/"))
            {
                cleaner.GoToCartPageIfItemsExistAndClearCart();
            }
            if (currentURL == "https://rahulshettyacademy.com/client/dashboard")
            {
                cleaner.SignOut();
            }

            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
            }

        }

        [OneTimeTearDown]
        public void AfterClass()
        {
            // Flush the report after all tests in the class have been executed
            ExtentReportHelper.FlushReport();
        }
    }
}
