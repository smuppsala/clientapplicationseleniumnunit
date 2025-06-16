using ClientApplication.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClientApplication.Utilities
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

    }
}
