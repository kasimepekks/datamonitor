using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;

namespace RLDA_VehicleData_Watch.Controllers.ADF0979
{
    public class PredictionController : Controller
    {
        private readonly ILogger<PredictionController> _logger;
        private IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly IConfiguration _configuration;
        private readonly string PredictionRequired;
        private readonly string PredictionVehicleID;
        public PredictionController(IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, IConfiguration configuration, ILogger<PredictionController> logger)
        {
            
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            _configuration = configuration;
            this._logger = logger;
            PredictionRequired = _configuration["DataPrediction:PredictionRequired"];
            PredictionVehicleID = _configuration["DataPrediction:PredictionVehicleID"];

        }
        /// <summary>
        /// 预测损伤达到GD水平的时间
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> WFTDamageCumulation(string startdate, string enddate,string vehicleid)
        {
            try
            {
                if (PredictionRequired == "true")
                {
                    if (PredictionVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var damagelist = await _IAnalysisData_WFT_Service.LoadWFTDamageCumulation(sd, ed, vehicleid);
                        return Json(damagelist);
                    }
                    else
                    {
                        return Json("No");
                    }
                }
                else
                {
                    return Json("No");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("PredictionController中WFTDamageCumulation方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
            
        }
    }
}
