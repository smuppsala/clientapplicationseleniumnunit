using ClientApplication.Models;
using ClientApplication.Pages;
using ClientApplication.Utilities;
using System.Text.Json;

namespace ClientApplication.Tests
{
    public class ClientLoginTests : TestBase
    {
        private ClientLoginPage _loginPage;
        private const string TestDataPath = @"Data\LoginTestData.json";
        public static IEnumerable<LoginTestModel> ValidLoginData => JsonDataReader.GetValidLogins(TestDataPath);
        public static IEnumerable<LoginTestModel> InvalidLoginData => JsonDataReader.GetInvalidLogins(TestDataPath);

        [SetUp]
        public void TestSetup()
        {
            _loginPage = new ClientLoginPage(Driver);
            _loginPage.GoTo();
        }

        [Test, TestCaseSource(nameof(ValidLoginData))]
        public void ValidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);
            Assert.IsTrue(_loginPage.IsSignOutVisible(), "Signout button should be visible after login.");
        }

        // This test will fail because error message population and disappearing is very quick. 
        [Test, TestCaseSource(nameof(InvalidLoginData))]
        public void InvalidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);

            bool stillLoginBtnDisplayed = _loginPage.StillInLoginPage();

            Assert.IsTrue(stillLoginBtnDisplayed, "Login button should be visible after invalid login");
        }

        [Test]
        public void TestLoginWithJsonData()
        {
            // this method has written before putting into a generic json data read method
            string jsonFileName = "LoginTestData.json";
            var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", jsonFileName);
            var jsonString = File.ReadAllText(jsonFilePath);

            var loginModels = JsonSerializer.Deserialize<List<LoginTestModel>>(jsonString);

            // Get the first valid login details
            var validLogin = loginModels.FirstOrDefault(m => m.IsValid);

            _loginPage.Login(validLogin.Email, validLogin.Password);

            // Assert login result (customize as needed)
            Assert.IsTrue(_loginPage.IsSignOutVisible(), "Signout button should be visible after login.");
        }

        [Test]
        public void TestLoginWithJsonDataUsingGenericMethod()
        {
            // Calling json file reader methos and putting data into LoginTestModels
            List<LoginTestModel> loginModels = JsonDataReader.LoadModels<LoginTestModel>(TestDataPath);

            // Get the first valid login details
            var validLogin = loginModels.FirstOrDefault(m => m.IsValid);

            _loginPage.Login(validLogin.Email, validLogin.Password);

            // Assert login result (customize as needed)
            Assert.IsTrue(_loginPage.IsSignOutVisible(), "Signout button should be visible after login.");
        }
    }
}
