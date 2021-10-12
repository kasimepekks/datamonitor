using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RLDA_VehicleData_Watch.Controllers
{
    [Authorize]
    public class PopUpController : Controller
    {
        public IActionResult LogPopUp()
        {
            return View();
        }
    }
}
