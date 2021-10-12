using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using RLDA_VehicleData_Watch.Models;

namespace RLDA_VehicleData_Watch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly datawatchContext _db;
       
        
        public HomeController(ILogger<HomeController> logger, datawatchContext db)
        {
            _logger = logger;
            _db = db;
          
           
        }
        
        public IActionResult Login()
        {
            
            return View();
        }
        [Authorize]
        public IActionResult Index()
        {
            ViewBag.User = HttpContext.Session.GetString("UserID");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        /// <summary>
        /// 去掉左边导航条
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult HomePage()
        {
            ViewBag.User = HttpContext.Session.GetString("UserID");
            return View();
        }


        public IActionResult GetVehicleID()
        {
            var vehiclelist = _db.Set<Vehicletable>().Where(a => a.State == 1).Select(b => b.VehicleId);
            return Json(vehiclelist);
        }
    }
}
