using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplicationTestProject.Config
{
    public static class EnvironmentConfig
    {
        public static string Url { get; } = "https://rahulshettyacademy.com/client";

       /* static EnvironmentConfig() 
        {
            Url = ConfigurationManager.AppSettings["BaseUrl"];
            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new ConfigurationErrorsException("BaseUrl is missing or empty in App.config.");
            }
        }*/
    }
}
