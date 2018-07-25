using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_sample.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {   
            return Content("index");
        }


        [HttpGet]
        public IActionResult AllUsers()
        {
            return Content("AllUsers");
        }

        [HttpGet]
        [Authorize(Policy = "HasDob")]
        public IActionResult PolicyOnly()
        {
            return Content("PolicyOnly");
        }
    }
}
