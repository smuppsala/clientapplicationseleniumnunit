using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace ClientApplicationTestProject.Listeners
{
    public interface ILoginListener
    {
        void OnLoginSuccess(IWebDriver driver);
        void OnLoginFailure(IWebDriver driver, string errorMessage);
    }
}
