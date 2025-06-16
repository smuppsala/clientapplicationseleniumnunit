using ClientApplication.Models;
using ClientApplication.Pages;
using ClientApplication.Utilities;


namespace ClientApplication.Tests
{
    public class ClientLoginTests : TestBase
    {
        private ClientLoginPage _loginPage;
        [SetUp]
        public void TestSetup()
        {
            _loginPage = new ClientLoginPage(Driver);
            _loginPage.GoTo();
        }

        private const string TestDataPath = @"Data\LoginTestData.json";
        public static IEnumerable<LoginTestModel> ValidLoginData => JsonDataReader.GetValidLogins(TestDataPath);
        public static IEnumerable<LoginTestModel> InvalidLoginData => JsonDataReader.GetInvalidLogins(TestDataPath);



        [Test, TestCaseSource(nameof(ValidLoginData))]
        public void ValidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);
            Assert.IsTrue(_loginPage.IsSignOutVisible(), "Signout button should be visible after login.");
        }

        // This test will fail because error message population and disappering is very quick. 
        [Test, TestCaseSource(nameof(InvalidLoginData))]
        public void InvalidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);
            string error = _loginPage.GetErrorText();

            Assert.That(error.Contains("Incorrect email or password."), Is.True, "Expected error message for invalid login.");

        }


    }
}
