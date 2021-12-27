using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Login()
        {
            
            return View();
        }
        //[Authorize]
        //public IActionResult Index()
        //{
        //    ViewBag.User = HttpContext.Session.GetString("UserID");
        //    return View();
        //}

        [Authorize]
        public IActionResult VehicleSetup()
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

        [Authorize]
        public IActionResult GetVehicleID()
        {
            var vehiclelist = _db.Set<Vehicletable>().Where(a => a.State == 1).Select(b => b.VehicleId);
            return Json(vehiclelist);
        }
        /// <summary>
        /// 获取数据库中的车辆设置表并返回给前端进行表格显示
        /// </summary>
        /// <returns></returns>
        
        public IActionResult GetVehicleSetup(int page, int limit)
        {
            var vehiclelist = _db.Set<Vehicletable>().OrderBy(a=>a.Id).Skip((page - 1) * limit).Take(limit);
            var count = vehiclelist.Count();
            return Json(new { code = 0, count = count, data = vehiclelist, msg = "" });
        }

        public IActionResult AddorEditSingleVehicleData(int id, string method, string vehicleidtext, string countrytext, byte statetext, string remarkstext, string areatext)
        {
            

            if (method == "edit")
            {
                Vehicletable vehicle = new Vehicletable()
                {
                    Id = id,
                    VehicleId = vehicleidtext,
                    Country = countrytext,
                    State = statetext,
                    Remarks = remarkstext,
                    Area = areatext

                };
                _db.Entry<Vehicletable>(vehicle).State = EntityState.Modified;
                if (_db.SaveChanges() > 0)
                {
                    return Content("编辑成功！");
                }
                else
                {
                    return Content("编辑失败! ");
                }
                    
            }
            else
            {
                Vehicletable vehicle = new Vehicletable()
                {
                    
                    VehicleId = vehicleidtext,
                    Country = countrytext,
                    State = statetext,
                    Remarks = remarkstext,
                    Area = areatext

                };
                _db.Set<Vehicletable>().Add(vehicle);
                if (_db.SaveChanges() > 0)
                {
                    return Content("添加成功！");
                }
                else
                {
                    return Content("添加失败! ");
                }

            }
        }

        public IActionResult DeleteSingleVehicleData(int id)
        {
           var vehicle= _db.Set<Vehicletable>().Where(a => a.Id == id).FirstOrDefault();
            _db.Entry<Vehicletable>(vehicle).State = EntityState.Deleted;
            if (_db.SaveChanges() > 0)
            {
                return Content("删除成功！");
            }
            else
            {
                return Content("删除失败! ");
            }
        }

    }
}
