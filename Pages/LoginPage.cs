using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ClientApplicationTestProject.Listeners;

namespace ClientApplicationTestProject.Pages
{
    public class LoginPage : BasePage
    {

        // Locators
        private By EmailInput => By.Id("userEmail");
        private By PasswordInput => By.Id("userPassword");
        private By LoginButton => By.Id("login");
        private By ErrorMessage => By.CssSelector(".toast-container.toast-message");

        //Actions delegated for login events
        public Action<bool,string> OnLoginAttemptCompleted { get; set; }

        //Add login listner property
        public ILoginListener LoginListener { get; set; }

        // Constructor 
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        // Navigate to the login page
        public void GoTo()
        {
            var baseUrl = EnvironmentConfig.Url;
            Driver.Navigate().GoToUrl(baseUrl);
        }

        // Actions
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

        public DashboardPage LoginWithSecrets()
        {
            EnterEmail(EnvironmentConfig.UserEmail);
            EnterPassword(EnvironmentConfig.Password);
            ClickLogin();
            return new DashboardPage(Driver);
        }

        // Assertions / Checks 
        public string GetErrorText() => WaitForToastAndGetText(ErrorMessage);

        public string GetTextInEmailField() => WaitGetElementText(EmailInput);

        public string GetTextInPasswordField() => WaitGetElementText(PasswordInput);

        public bool StillInLoginPage()
        {
            var element = WaitForElementVisible(LoginButton);

            // Also check the URL to confirm we're still on the login page
            bool urlIndicatesLoginPage = Driver.Url.Contains("login") || Driver.Url.EndsWith("/");

            return element.Displayed && urlIndicatesLoginPage;
           
        }
    }
    //public static class WebDriverWaitExtensions
    //{
    //    public static IWebElement WaitForElementVisible(this WebDriverWait wait, By locator)
    //    {
    //        return wait.Until(driver =>
    //        {
    //            var element = driver.FindElement(locator);
    //            return element.Displayed ? element : null;
    //        });
    //    }
    //}
}
