using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MyMvc.Controllers
{
    public class SettingsController : Controller
    {
        private readonly IConfiguration _config;
        public SettingsController(IConfiguration config)
        {
            _config = config;
        }
        public IActionResult Index()
        {
            ViewData["ConnectionString"] = _config.GetConnectionString("WideWorldImporters");

            return View();
        }
    }
}