using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Utilities;
using Microsoft.Testing.Platform.Configurations;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Flows
{
    public class LoginFlow
    {
        private readonly IWebDriver _driver;
        public const string jsonPath = @"Data\LoginTestData.json";

        public LoginFlow(IWebDriver driver) 
        {
            _driver = driver;
        }

        public DashboardPage LoginAsValidUser() 
        {
            var loginPage = new LoginPage(_driver);
            loginPage.GoTo();

            var validCreds = JsonDataReader.GetValidLogins(jsonPath).First();
            loginPage.Login(validCreds.Email, validCreds.Password);

            // Assert login succeeded
            Assert.That(loginPage.IsSignOutVisible(),Is.True, "Expected signout button to be visible after valid login.");

            return new DashboardPage(_driver);
        }
    }
}
