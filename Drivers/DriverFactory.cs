using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace ClientApplicationTestProject.Drivers
{
    public static class DriverFactory
    {
       /* public static IWebDriver GetDriver()
        {
            string? browser = ConfigurationManager.AppSettings["Browser"];
            if(browser != null)

            return browser.ToLower() switch
            {
                "chrome" => new ChromeDriver(),
                "firefox" => new FirefoxDriver(),
                _ => throw new NotSupportedException($"Browser '{browser}' is not supported")
                
            };
        } */


    }
}
