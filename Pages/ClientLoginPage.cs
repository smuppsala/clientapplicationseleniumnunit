using System.Configuration;
using System.Collections.Specialized;
using NUnit.Framework;
using System.Configuration;
using OpenQA.Selenium;
using ClientApplicationTestProject.Drivers;
using Microsoft.Testing.Platform.Configurations;
using ClientApplicationTestProject.Config;

namespace ClientApplicationTestProject.Pages
{
    public class ClientLoginPage : BasePage
    {

        public ClientLoginPage(IWebDriver driver) : base(driver)
        {
        }

        // Locators
        private By EmailInput => By.Id("userEmail");
        private By PasswordInput => By.Id("userPassword");
        private By LoginButton => By.Id("login");
        private By ErrorMessage => By.CssSelector(".toast-container.toast-message");
        private By SignOutButton => By.XPath("//button[text()=' Sign Out ']");
        private By HomeButton => By.XPath("//button[text()=' HOME ']");

        // Navigate to the login page
        public void GoTo()
        {
            var baseUrl = EnvironmentConfig.Url;
            Driver.Navigate().GoToUrl(baseUrl);

            //var projectRoot = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\.."));
            //var screenshotsDir = System.IO.Path.Combine(projectRoot, "Screenshots");
        }

        // Fill in login form
        public void EnterEmail(string email) => WaitClearAndEnterText(EmailInput, email);
        public void EnterPassword(string password) => WaitClearAndEnterText(PasswordInput, password);

        public void ClickLogin() => WaitAndClick(LoginButton);

        public void Login(string email, string password) 
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickLogin();
        }

        // public string GetErrorText() => WaitForElementVisible(ErrorMessage).Text;
         public string GetErrorText() => WaitGetElementText(ErrorMessage);
        public bool IsSignOutVisible() => WaitForElementVisible(SignOutButton).Displayed;

        //returning multiple elements from Method using Tuples 
        public (bool homeDisplayed, bool signoutDisplayed) IsLoggedIn() 
        {
            return (WaitForElementVisible(HomeButton).Displayed, WaitForElementVisible(SignOutButton).Displayed);
        }

        public string GetTextInEmailField() 
        {

            return WaitGetElementText(EmailInput);
        }

        public string GetTextInPasswordField()
        {
            return WaitGetElementText(PasswordInput);
        }

        public bool StillInLoginPage() 
        {
            return WaitForElementVisible(LoginButton).Displayed;
        }
    }
}
