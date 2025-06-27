using ClientApplicationTestProject.Models;
using Newtonsoft.Json;

namespace ClientApplicationTestProject.Utilities
{
    public static class JsonDataReader
    {
        public static List<LoginTestModel> LoadData(string jsonPath)
        {
            var fullPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, jsonPath);
            var jsonContent = File.ReadAllText(fullPath);
            return JsonConvert.DeserializeObject<List<LoginTestModel>>(jsonContent);
        }

        public static IEnumerable<LoginTestModel> GetValidLogins(string jsonPath)
        {
            return LoadData(jsonPath).Where(data => data.IsValid);
        }

        public static IEnumerable<LoginTestModel> GetInvalidLogins(string jsonPath)
        {
            return LoadData(jsonPath).Where(data => !data.IsValid);
        }

        public static List<T> LoadModels<T>(string relativeFilePath)
        {
            var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeFilePath);
            var jsonString = File.ReadAllText(jsonFilePath);
            return System.Text.Json.JsonSerializer.Deserialize<List<T>>(jsonString);
        }

    }
}
