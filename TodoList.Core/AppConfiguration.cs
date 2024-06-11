using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Core
{
    public static class AppConfiguration
    {
        public const string PolicyName = "AppCors";
        public static string BackEnd { get; set; } = "http://localhost:5142";
        public static string FrontEnd { get; set; } = "http://localhost:5080";
    }
}
