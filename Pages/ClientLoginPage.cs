
using OpenQA.Selenium;

namespace ClientApplication.Pages
{
    public class ClientLoginPage : BasePage
    {

        public ClientLoginPage(IWebDriver driver) : base(driver) { }

        // Page URL
        public string Url => "https://rahulshettyacademy.com/client";

        // Locators
        private By EmailInput => By.Id("userEmail");
        private By PasswordInput => By.Id("userPassword");
        private By LoginButton => By.Id("login");
        private By ErrorMessage => By.CssSelector(".toast-container.toast-message");
        private By LogoutButton => By.XPath("//button[text()=' Sign Out ']");

        // Navigate to the login page
        public void GoTo() => Driver.Navigate().GoToUrl(Url);

        // Fill in login form
        public void EnterEmail(string email) => WaitClearAndEnterText(EmailInput, email);
        public void EnterPassword(string password) => WaitClearAndEnterText(PasswordInput, password);

        //  public void EnterEmail(string email) => Driver.FindElement(EmailInput).SendKeys(email);
        // public void EnterPassword(string pwd) => Driver.FindElement(PasswordInput).SendKeys(pwd);

        // Submit the form
        // public void ClickLogin() => Driver.FindElement(LoginButton).Click();
        public void ClickLogin() => WaitAndClick(LoginButton);

        public void Login(string email, string password) 
        {
            EnterEmail(email);
            EnterPassword(password);
            ClickLogin();
        }

        // public string GetErrorText() => WaitForElementVisible(ErrorMessage).Text;
         public string GetErrorText() => WaitGetElementText(ErrorMessage);
        public bool IsSignOutVisible() => WaitForElementVisible(LogoutButton).Displayed;
    }
}
