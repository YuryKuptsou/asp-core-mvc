using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace aspCoreMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var i = 0;
            var s = 3 / i;

            return View();
        }
    }
}