using System.Data;
using System.Text.Json;
using ClientApplicationTestProject.Models;
using ClientApplicationTestProject.Pages;
using ClientApplicationTestProject.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ClientApplicationTestProject.Tests
{
    public class ClientLoginTests : TestBase
    {
        private LoginPage _loginPage;
        private const string TestDataPath = @"Data\LoginTestData.json";
        public static IEnumerable<LoginTestModel> ValidLoginData => JsonDataReader.GetValidLogins(TestDataPath);
        public static IEnumerable<LoginTestModel> InvalidLoginData => JsonDataReader.GetInvalidLogins(TestDataPath);


        [SetUp]
        public void TestSetup()
        {
            _loginPage = new LoginPage(Driver);
            _loginPage.GoTo();
        }

        [Test, TestCaseSource(nameof(ValidLoginData)), Order(2)]
        public void ValidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);
            Assert.That(_loginPage.IsSignOutVisible(), Is.True, "Signout button should be visible after login.");

        }

        // This test will fail because error message population and disappearing is very quick. 
        [Test, TestCaseSource(nameof(InvalidLoginData)), Order(1)]
        public void InvalidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);

            bool stillLoginBtnDisplayed = _loginPage.StillInLoginPage();

            Assert.That(stillLoginBtnDisplayed, Is.True, "Login button should be visible after invalid login");
        }

       /* [Test, Order(3)]
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
            Assert.That(_loginPage.IsSignOutVisible(), Is.True, "Signout button should be visible after login.");
        }*/

        [Test, Order(3)]
        public void TestLoginWithJsonDataUsingGenericMethod()
        {
            // Calling json file reader methos and putting data into LoginTestModels
            List<LoginTestModel> loginModels = JsonDataReader.LoadModels<LoginTestModel>(TestDataPath);

            // Get the first valid login details
            var validLogin = loginModels.FirstOrDefault(m => m.IsValid);

            _loginPage.Login(validLogin.Email, validLogin.Password);

            // Assert login result (customize as needed)
            var getLoggedIn = _loginPage.IsLoggedIn();
            Assert.That(getLoggedIn.homeDisplayed && getLoggedIn.signoutDisplayed, " Home and Signout buttons should be visible after login.");
        }

        [Test, Order(4)]
        public void LoginWithExcelData()
        {
            string filePath = @"Data/LoginData.xlsx";
            //Populate data to a data table
            DataTable table = ExcelReader.ExcelToDataTable(filePath);

            //reading userEmail and Paasword from data table
            string userEmail = table.Rows[0][0].ToString();
            string password = table.Rows[0][1].ToString();

            //print data 
            Console.WriteLine($"UserEmail: {userEmail} and Password: {password}");
            _loginPage.Login(userEmail, password);

            var getLoggedIn = _loginPage.IsLoggedIn();
            Assert.That(getLoggedIn.homeDisplayed && getLoggedIn.signoutDisplayed, " Home and Signout buttons should be visible after login.");

        }
        [Test]
        public void LoginWithConfigurationData()
        {
            _loginPage.LoginWithDefaultCredentials();
            var getLoggedIn = _loginPage.IsLoggedIn();
            Assert.That(getLoggedIn.homeDisplayed && getLoggedIn.signoutDisplayed, " Home and Signout buttons should be visible after login.");

        }

        [Test]
        public void VerifyLoginWithUserSecrets()
        {
            // Setup - Output the credentials being used (without the full password)
            string userEmail = EnvironmentConfig.UserEmail.Length > 0 ?
               new string('*', EnvironmentConfig.UserEmail.Length - 2) +
                                   EnvironmentConfig.UserEmail.Substring(EnvironmentConfig.Password.Length - 2) :
                                   "null";
            string maskedPassword = EnvironmentConfig.Password?.Length > 0 ?
                                   new string('*', EnvironmentConfig.Password.Length - 2) +
                                   EnvironmentConfig.Password.Substring(EnvironmentConfig.Password.Length - 2) :
                                   "null";

            Console.WriteLine($"Testing login with: {userEmail} / {maskedPassword}");

            // Test source by comparing with appsettings
            using (StreamReader r = new StreamReader("appsettings.json"))
            {
                string json = r.ReadToEnd();
                var appsettings = JsonConvert.DeserializeObject<JObject>(json);
                string appsettingsEmail = appsettings?["TestSettings"]?["Credentials"]?["UserEmail"]?.ToString();

                // If different, likely from User Secrets
                bool usingUserSecrets = userEmail != appsettingsEmail && !string.IsNullOrEmpty(userEmail);
                Console.WriteLine($"Using User Secrets: {usingUserSecrets}");
            }

            // Act
            _loginPage.LoginWithDefaultCredentials();

            // Assert
            var loginStatus = _loginPage.IsLoggedIn();
            Assert.That(loginStatus.homeDisplayed && loginStatus.signoutDisplayed,
                        "Home and Signout buttons should be visible after login with User Secrets credentials.");
        }


    }
}
