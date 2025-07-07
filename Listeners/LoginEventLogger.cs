using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using ClientApplicationTestProject.Utilities;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Listeners
{
    public class LoginEventLogger : ILoginListener
    {
        private readonly ExtentTest _test;

        public LoginEventLogger(ExtentTest test)
        {
            _test = test;
        }

        public void OnLoginSuccess(IWebDriver driver)
        {
            _test.Log(Status.Info, "Login successful");
        }

        public void OnLoginFailure(IWebDriver driver, string errorMessage) 
        {
            _test.Log(Status.Warning, $"Login field: {errorMessage}");

            //capture screenshot
            string screenshotPath = ExtentReportHelper.CaptureScreenshot(driver, "LoginFailure");
            _test.AddScreenCaptureFromPath(screenshotPath, "Login Failure Screenshot");
        }
    }
}
