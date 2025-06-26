using ClientApplication.Pages;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Pages
{
    public class MainMenuPage : BasePage
    {
        public MainMenuPage(IWebDriver driver) : base(driver) { }

        private By SignOutButton => By.XPath("//button[text()=' Sign Out ']");
        private By HomeButton => By.XPath("//button[text()=' HOME ']");

        public void SignOut() 
        {
            WaitAndClick(SignOutButton);
        }

    }
}
