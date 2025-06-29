using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using NUnit.Framework;
using System;
using System.IO;
using AventStack.ExtentReports.Reporter.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ClientApplicationTestProject.Utilities
{
    public class ExtentReportHelper
    {
        public static ExtentReports Extent;
        public static ExtentTest Test;
        private static readonly IConfiguration configuration;

        // Static constructor to initialize the static readonly field
        static ExtentReportHelper()
        {
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();
        }

        //get configuration values as defaults
        private static string GetSetting(string key, string defaultValue) =>
            configuration[$"TestSettings:{key}"] ?? defaultValue;
        
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
                var htmlReporter = new ExtentSparkReporter(reportFilePath); // Ensure ExtentHtmlReporter is resolved

                //use configuration values
                string reportTheme = GetSetting("ReportTheme","Dark");
                htmlReporter.Config.Theme = reportTheme.ToLower() == "dark" ? Theme.Dark : Theme.Standard;
                htmlReporter.Config.DocumentTitle = GetSetting("ReportTitle" ,"Test Automation Report");
                htmlReporter.Config.ReportName = "Client Application Test Results";
                htmlReporter.Config.TimelineEnabled = true;

                Extent = new ExtentReports();
                Extent.AttachReporter(htmlReporter);
                Extent.AddSystemInfo("Application", "Client Application");
                Extent.AddSystemInfo("Environment", GetSetting("Environment", "QA"));
                Extent.AddSystemInfo("Framework", ".NET 8.0");
                Extent.AddSystemInfo("Browser", GetSetting("Browser", "Chrome"));
                Extent.AddSystemInfo("Base URL", GetSetting("BaseUrl", "https://rahulshettyacademy.com/client"));
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
