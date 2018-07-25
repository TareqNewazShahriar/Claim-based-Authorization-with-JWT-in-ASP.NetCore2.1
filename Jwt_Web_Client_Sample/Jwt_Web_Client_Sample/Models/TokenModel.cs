using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt_Web_Client_Sample.Models
{
    public class TokenModel
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
