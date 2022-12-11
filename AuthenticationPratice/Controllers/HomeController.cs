using AuthenticationPratice.Extensions;
using AuthenticationPratice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthenticationPratice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // 取出所有claim的值
            var identity = User.Identity as ClaimsIdentity;
            var claims = identity.Claims.Select(x => x.Value).ToList();

            // 其實只要有Id 就可以取得使用者資料
            var userId = User.GetNameIdentity();
            Console.WriteLine(userId);
            Console.WriteLine(User.Identity.Name);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
