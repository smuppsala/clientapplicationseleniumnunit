using ClientApplicationTestProject.Listeners;
using ClientApplicationTestProject.Models;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Utilities;


namespace ClientApplicationTestProject.Tests
{
    public class LoginTests : TestBase
    {
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.GoTo();
        }

        [Test, Order(1)]
        public void LoginWithConfigurationData()
        {
            _dashboardPage = _loginPage.LoginWithSecrets();
            var hasLoggedIn = _dashboardPage.HasLoggedIn();
            Assert.That(hasLoggedIn.homeDisplayed && hasLoggedIn.signoutDisplayed, " Home and Signout buttons should be visible after login.");
        }

        // This test will fail because error message population and disappearing is very quick. 
        [Test, Order(2)]
        public void InvalidLoginTest()
        {
            string randomEmail = TestDataGenerator.GenerateRandomEmail();
            string randomPassword = TestDataGenerator.GenerateRandomPassword();
            _loginPage.Login(randomEmail, randomPassword);
            bool stillLoginBtnDisplayed = _loginPage.StillInLoginPage();
            Assert.That(stillLoginBtnDisplayed, Is.True, "Login button should be visible after invalid login");
        }



    }
}
