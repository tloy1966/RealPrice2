using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RealPrice.Controllers
{
    public class forecastController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}