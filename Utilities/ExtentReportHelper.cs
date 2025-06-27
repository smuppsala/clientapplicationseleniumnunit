using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Configuration;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Utilities
{
    public class ExtentReportHelper
    {
        public static ExtentReports Extent;
        public static ExtentTest Test;

        public static void InitializeReport() 
        {
            if (Extent == null)
            {
                // Create report directory if doesn't exist
                string reportPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "ExtentReports");
                if (!Directory.Exists(reportPath)) 
                {
                    Directory.CreateDirectory(reportPath);
                }

                // Initialize Extent Report
                string reportFilePath = Path.Combine(reportPath, $"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.html");
                var htmlReporter = new ExtentHtmlReporter(reportFilePath);

                htmlReporter.Config.Theme = Theme.Dark;
                htmlReporter.Config.DocumentTitle = "Test Automation Report";
                htmlReporter.Config.ReportName = "Client Application Test Results";
                htmlReporter.Config.EnableTimeline = true;

                Extent = new ExtentReports();
                Extent.AttachReporter(htmlReporter);
                Extent.AddSystemInfo("Application", "Client Application");
                Extent.AddSystemInfo("Environment", "QA");
                Extent.AddSystemInfo("Framework", ".NET 8.0");
                Extent.AddSystemInfo("Browser", "Chrome");
            }
        }

        public static void FlushReport() 
        {
            Extent?.Flush();
        }

        public static string CaptureScreenshot(IWebDriver driver, string screenshotName)
        {
            string screenshotPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "ExtentReports", "Screenshots");
            if (!Directory.Exists(screenshotPath))
            {
                Directory.CreateDirectory(screenshotPath);
            }

            string fileName = $"{screenshotName}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            string filePath = Path.Combine(screenshotPath, fileName);

            var screenshotDriver = driver as ITakesScreenshot;
            if (screenshotDriver != null)
            {
                var screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(filePath);
            }
            return filePath;
        }
    }

    
}
