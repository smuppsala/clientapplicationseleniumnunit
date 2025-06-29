using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace ClientApplicationTestProject.Drivers
{
    public static class DriverFactory
    {
        private static readonly IConfiguration configuration;

        static DriverFactory()
        {
            //build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static IWebDriver GetDriver() 
        {
            string browser = configuration["TestSettings:Browser"] ?? "Chrome";
            return browser.ToLower() switch
            {
                "chrome" => new ChromeDriver(),
                "firefox" => new FirefoxDriver(),
                _ => throw new NotSupportedException($"Browser '{browser}' is not supported")
            };
        }



    }
}
