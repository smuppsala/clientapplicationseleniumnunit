using NUnit.Framework;
using OpenQA.Selenium;
using ClientApplicationTestProject.Drivers;
using Microsoft.Testing.Platform.Configurations;

namespace ClientApplicationTestProject.Pages
{
    public class LoginPage : BasePage
    {

        public LoginPage(IWebDriver driver) : base(driver)
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
        }

        // Fill in login form
        public void EnterEmail(string email) => WaitClearAndEnterText(EmailInput, email);
        public void EnterPassword(string password) => WaitClearAndEnterText(PasswordInput, password);

        public void ClickLogin() => WaitAndClick(LoginButton);

        public bool LoginWithDefaultCredentials()
        {
            // Validate credentials exist
            if (string.IsNullOrEmpty(EnvironmentConfig.UserEmail))
            {
                Console.WriteLine("ERROR: UserEmail missing in configuration!");
                throw new InvalidOperationException("Email credential is missing. Check User Secrets configuration.");
            }
            if(string.IsNullOrEmpty(EnvironmentConfig.Password))
    {
                Console.WriteLine("ERROR: Password missing in configuration!");
                throw new InvalidOperationException("Password credential is missing. Check User Secrets configuration.");
            }

            Console.WriteLine($"Using credentials from configuration");
            EnterEmail(EnvironmentConfig.UserEmail);
            EnterPassword(EnvironmentConfig.Password);
            ClickLogin();
            return true;
        }


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
