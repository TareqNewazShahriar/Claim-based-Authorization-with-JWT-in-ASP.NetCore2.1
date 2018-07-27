using Jwt_Web_Client_Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt_Web_Client_Sample
{
    public class AppData
    {
        public static string ApiUrl = "https://localhost:44393/";

        public const string TokenName = "token";

        public const string LoginPath = @"/account/login";

        public static UserModel iModel = new UserModel();
    }
}
