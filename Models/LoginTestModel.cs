using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApplicationTestProject.Models
{
    public class LoginTestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsValid { get; set; }
    }
}
