using ClientApplicationTestProject.Models;
using ClientApplicationTestProject.Pages;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using ClientApplicationTestProject.Drivers;
using ClientApplicationTestProject.Utilities;
using System.Data;


namespace ClientApplicationTestProject.Tests
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

        [Test, TestCaseSource(nameof(ValidLoginData)), Order(2)]
        public void ValidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);
            Assert.That(_loginPage.IsSignOutVisible(), Is.True,"Signout button should be visible after login.");
            
        }

        // This test will fail because error message population and disappearing is very quick. 
        [Test, TestCaseSource(nameof(InvalidLoginData)), Order(1)]
        public void InvalidLoginTest(LoginTestModel data)
        {
            _loginPage.Login(data.Email, data.Password);

            bool stillLoginBtnDisplayed = _loginPage.StillInLoginPage();

            Assert.That(stillLoginBtnDisplayed,Is.True, "Login button should be visible after invalid login");
        }

        [Test, Order(3)]
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
            Assert.That(_loginPage.IsSignOutVisible(),Is.True, "Signout button should be visible after login.");
        }

        [Test, Order(4)]
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

        [Test, Order(5)]
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
    }
}
