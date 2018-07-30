using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_sample
{
    public class AppData
    {
        /// <summary>
        /// For dev: in minutes // For Prod: in days
        /// </summary>
#if DEBUG
        public static int ExpiresIn = 5;
#else
        public static int ExpiresIn = 7;
#endif
        
    }
}
