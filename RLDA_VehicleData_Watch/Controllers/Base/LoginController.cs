using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MysqlforDataWatch;
using RLDA_VehicleData_Watch.Models;
using Tools;

namespace RLDA_VehicleData_Watch.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public IActionResult Login(string user, string key)
        {
         
            var LoginStr = "";
            var md5key = MD5.Sign(key, "utf-8");
            SysAuthority sys_Authority = LoginClass.Login(user, md5key);
            if (sys_Authority != null)
            {
                //数据库匹配后把user和key信息放入claim里
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user),
                    new Claim("Pwd",key)
                };
                var identity = new ClaimsIdentity(claims, "UserorAdmin");
                HttpContext.SignInAsync(new ClaimsPrincipal(identity));
                //创建session
                HttpContext.Session.SetString("UserID", user);
               
               
                if (user == "Admin")
                {

                    LoginStr = "AdminSuceess";
                }
                else
                {
                    LoginStr = "UserSuceess";
                }
            }
            else
            {
                LoginStr = "Fail";
            }
            return Json(LoginStr);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();//退出授权
            return RedirectToAction("Login", "Home");

        }

    }
}
