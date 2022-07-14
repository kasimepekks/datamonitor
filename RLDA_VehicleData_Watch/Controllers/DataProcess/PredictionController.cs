using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;//内存缓存
        public PredictionController(IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, IConfiguration configuration, ILogger<PredictionController> logger,IMemoryCache memoryCache)
        {
            
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            _configuration = configuration;
            this._logger = logger;
            PredictionRequired = _configuration["DataPrediction:PredictionRequired"];
            PredictionVehicleID = _configuration["DataPrediction:PredictionVehicleID"];
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 预测损伤达到GD水平的时间
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> WFTDamageCumulation(string startdate, string enddate,string vehicleid)
        {
            var cash = await _memoryCache.GetOrCreateAsync<JsonResult>("WFTDamageCumulation" + startdate + enddate + vehicleid, async value =>
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
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            });
            if (cash != null)
            {
                return cash;
            }
            else
            {
                return Json("No");
            }

        }
    }
}
