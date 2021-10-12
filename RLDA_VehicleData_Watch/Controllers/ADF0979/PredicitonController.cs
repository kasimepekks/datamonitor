using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using MysqlforDataWatch;

namespace RLDA_VehicleData_Watch.Controllers.ADF0979
{
    public class PredicitonController : Controller
    {
        private readonly datawatchContext _DB;
        private IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        public PredicitonController(IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service,datawatchContext db)
        {
            
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
           
            _DB = db;
          
        }
        /// <summary>
        /// 预测损伤达到GD水平的时间
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public IActionResult WFTDamageCumulation(string startdate, string enddate)
        {
            var sd = Convert.ToDateTime(startdate);
            var ed = Convert.ToDateTime(enddate);
            var damagelist = _IAnalysisData_WFT_Service.LoadWFTDamageCumulation(sd, ed);
            return Json(damagelist);
        }
    }
}
