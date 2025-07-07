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
            bool headless = bool.TryParse(configuration["TestSettings:Headless"], out bool result) && result;
            
            return browser.ToLower() switch
            {
                "chrome" => CreateChromDriver(headless),
                "firefox" => CreateFirefoxDriver(headless),
                _ => throw new NotSupportedException($"Browser '{browser}' is not supported")
            };
        }

        private static IWebDriver CreateChromDriver(bool headless)
        {
            var options = new ChromeOptions();

            if (headless) 
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }
            return new ChromeDriver(options);
        }

        private static IWebDriver CreateFirefoxDriver(bool headless)
        {
            var options = new FirefoxOptions();

            if (headless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }
            return new FirefoxDriver(options);
        }
    }
}
