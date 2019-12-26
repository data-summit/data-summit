using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DataSummitWeb.Controllers
{
    public class OAuthTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}