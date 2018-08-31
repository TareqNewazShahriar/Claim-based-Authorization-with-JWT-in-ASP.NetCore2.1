using Jwt_Web_Client_Sample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt_Web_Client_Sample.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var x = await GetAsync<string>("home/index");

            ViewBag.data = "from api: " + x;

            return View("_view");
        }

        public async Task<IActionResult> Authorized()
        {
            var x = await GetAsync<string>("home/allusers");

            ViewBag.data = "from api: " + x;

            return View("_view");
        }

        [Authorize]
        public async Task<IActionResult> Protected()
        {
            var x = await GetAsync<string>("home/policyonly");

            ViewBag.data = "from api: " + x;

            return View("_view");
        }

        [Authorize]
        public async Task<IActionResult> GetAllBookingRequests()
        {
            var x = await GetAsync<List<BookingRequestGetAll>>("bookingRequest/getall");

            ViewBag.data = JsonConvert.SerializeObject(x);

            return View("_view");
        }
    }
}
