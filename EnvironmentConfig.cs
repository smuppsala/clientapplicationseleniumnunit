using Microsoft.Extensions.Configuration;

namespace ClientApplicationTestProject
{
    public static class EnvironmentConfig
    {
        private static readonly IConfiguration configuration;

        static EnvironmentConfig()
        {
            // Debug information about current directory
            var currentDir = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current Directory: {currentDir}");

            // Get the user secrets ID from assembly
            var userSecretsIdAttribute = typeof(EnvironmentConfig).Assembly
                .GetCustomAttributes(typeof(Microsoft.Extensions.Configuration.UserSecrets.UserSecretsIdAttribute), false)
                .FirstOrDefault() as Microsoft.Extensions.Configuration.UserSecrets.UserSecretsIdAttribute;

            var secretsId = userSecretsIdAttribute?.UserSecretsId ?? "unknown";
            Console.WriteLine($"UserSecretsId: {secretsId}");

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
// .AddUserSecrets<Program>()
// To this (uses the current assembly)
.AddUserSecrets(typeof(EnvironmentConfig).Assembly)
                .AddEnvironmentVariables("TEST_") // Prefix for environment variables
                .Build();
            // Debug output
            Console.WriteLine($"UserEmail from config: {configuration["TestSettings:Credentials:UserEmail"]}");
            Console.WriteLine($"Password from config: {configuration["TestSettings:Credentials:Password"]}");
        }
        public static string Url =>
            configuration["TestSettings:BaseUrl"] ?? "https://rahulshettyacademy.com/client";

        public static string UserEmail =>
            configuration["TestSettings:Credentials:UserEmail"];

        public static string Password =>
             configuration["TestSettings:Credentials:Password"];
    }
}
