using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using Tools;
using Tools.AddDistance;

namespace RLDA_VehicleData_Watch.Controllers
{
    /// <summary>
    /// 此控制器主要功能为分析统计数据，在前端选择日期时间进行计算
    /// </summary>
    public class AnalysisController : Controller
    {
        //private readonly datawatchContext _DB;
        //private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly ILogger<AnalysisController> _logger;
        private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly IBrakeDistribution_IBLL _IBrakeDistribution_Service;
        private readonly IBumpDistribution_IBLL _IBumpDistribution_Service;

        private readonly ISpeedDistribution_ACC_IBLL _ISpeedDistribution_ACC_Service;
        private readonly IThrottleDistribution_IBLL _IThrottleDistribution_Service;
        private readonly ISteeringDistribution_IBLL _ISteeringDistribution_Service;

        private readonly IGPSRecord_IBLL _IGPSRecord_Service;

        private readonly IConfiguration _configuration;
        private readonly string AnalysisRequired;
        private readonly string AnalysisVehicleID;
        private readonly int reducetimeforgps;
        //private double Distance = 0;
        public AnalysisController(IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, IAnalysisData_ACC_IBLL IAnalysisData_ACC_Service, ISpeedDistribution_ACC_IBLL ISpeedDistribution_ACC_Service,IBrakeDistribution_IBLL IBrakeDistribution_Service, IBumpDistribution_IBLL IBumpDistribution_Service, ISteeringDistribution_IBLL ISteeringDistribution_Service, IGPSRecord_IBLL IGPSRecord_Service, IConfiguration configuration, IThrottleDistribution_IBLL IThrottleDistribution_Service, ILogger<AnalysisController> logger)
        {
            //_IAnalysisData_ACC_Service = IAnalysisData_ACC_Service;
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            _IAnalysisData_ACC_Service = IAnalysisData_ACC_Service;
            _ISpeedDistribution_ACC_Service = ISpeedDistribution_ACC_Service;
            _IBrakeDistribution_Service = IBrakeDistribution_Service;
            _IBumpDistribution_Service = IBumpDistribution_Service;
            _IThrottleDistribution_Service = IThrottleDistribution_Service;
            _ISteeringDistribution_Service = ISteeringDistribution_Service;
            _IGPSRecord_Service = IGPSRecord_Service;
            _configuration = configuration;
            this._logger = logger;
            //_DB = db;
            AnalysisRequired = _configuration["DataAnalysis:AnalysisRequired"];
            AnalysisVehicleID = _configuration["DataAnalysis:AnalysisVehicleID"];
            reducetimeforgps = Convert.ToInt32(_configuration["DataAnalysis:ReduceTimeforGPS"]);
        }
       
       
        /// <summary>
        /// 根据所选的时间范围来传给前端速度的总体分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> SpeedAnalysis(string startdate,string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var SpeedList = await _ISpeedDistribution_ACC_Service.LoadSpeedDistribution(sd, ed, vehicleid);
                        var json = Json(SpeedList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
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

                _logger.LogInformation("AnalysisController中SpeedAnalysis方法出现问题："+ex.Message+ "出现时间："+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("No");
            }
           
        }
        /// <summary>
        /// 根据所选的时间范围来传给前端每天的里程分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> SpeedPerDayAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var SpeedPerDayList = await _ISpeedDistribution_ACC_Service.LoadSpeedDistributionperday(sd, ed, vehicleid);
                        return Json(SpeedPerDayList);
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
                _logger.LogInformation("AnalysisController中SpeedPerDayAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");

            }
            

           
        }
        /// <summary>
        /// 根据所选的时间范围来传给前端每时的里程分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public async Task<IActionResult> SpeedPerHourAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var SpeedPerHourList = await _ISpeedDistribution_ACC_Service.LoadSpeedDistributionperhour(sd, ed, vehicleid);
                        return Json(SpeedPerHourList);
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
                _logger.LogInformation("AnalysisController中SpeedPerHourAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
           
           
        }

        public async Task<IActionResult> WFTDamageAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var damagelist = await _IAnalysisData_WFT_Service.LoadWFTDamage(sd, ed, vehicleid);
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
                _logger.LogInformation("AnalysisController中WFTDamageAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
         

          
        }

        public async Task<IActionResult> ACCandDisAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var accanddislist = await _IAnalysisData_ACC_Service.LoadACCandDisData(sd, ed, vehicleid);
                        return Json(accanddislist);
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
                _logger.LogInformation("AnalysisController中ACCandDisAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }



        }
        public async Task<IActionResult> BrakeDistributionAnalysis(string startdate, string enddate,string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var brakelist = await _IBrakeDistribution_Service.LoadBrakeDistribution(sd, ed, vehicleid);
                        return Json(brakelist);
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
                _logger.LogInformation("AnalysisController中BrakeDistributionAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
          

           
        }
        public async Task<IActionResult> BrakeCountAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var brakecount = await _IBrakeDistribution_Service.GetBrakeCount(sd, ed, vehicleid);
                        return Json(brakecount);
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
                _logger.LogInformation("AnalysisController中BrakeCountAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
           

         
        }


        public async Task<IActionResult> BumpDistributionAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var bumplist = await _IBumpDistribution_Service.LoadBumpDistribution(sd, ed, vehicleid);
                        return Json(bumplist);
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
                _logger.LogInformation("AnalysisController中BumpDistributionAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
            

           
        }
        public async Task<IActionResult> BumpCountAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var bumpcount = await _IBumpDistribution_Service.GetBumpCount(sd, ed, vehicleid);
                        return Json(bumpcount);
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
                _logger.LogInformation("AnalysisController中BumpCountAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return Json("No");
            }
           

           
        }

        public async Task<IActionResult> ThrottleAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var ThrottleList = await _IThrottleDistribution_Service.LoadThrottleDistribution(sd, ed, vehicleid);
                        
                        var json = Json(ThrottleList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
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

                _logger.LogInformation("AnalysisController中ThrottleAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("No");
            }

        }

        public async Task<IActionResult> SteeringAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var SteeringList = await _ISteeringDistribution_Service.LoadSteeringDistribution(sd, ed, vehicleid);
                        var json = Json(SteeringList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
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

                _logger.LogInformation("AnalysisController中SteeringAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("No");
            }

        }

        public async Task<IActionResult> GPSAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        //这里的reducetimeforgps是appsetting里的

                        var GPSList = await _IGPSRecord_Service.LoadGPSRecord(sd, ed, vehicleid, reducetimeforgps);
                        //var lat = GPSList.Select(a => a.Lat);
                        //var lon= GPSList.Select(a => a.Lon);
                        var json = Json(GPSList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
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

                _logger.LogInformation("AnalysisController中GPSAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("No");
            }

        }

        public async Task<IActionResult> TextperDayAnalysis(string startdate, string enddate, string vehicleid)
        {
            try
            {
                if (AnalysisRequired == "true")
                {
                    if (AnalysisVehicleID.Contains(vehicleid))
                    {
                        var sd = Convert.ToDateTime(startdate);
                        var ed = Convert.ToDateTime(enddate);
                        var textList = await _ISpeedDistribution_ACC_Service.LoadTextRecord(sd, ed, vehicleid);

                        var json = Json(textList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
                        return json;
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

                _logger.LogInformation("AnalysisController中TextperDayAnalysis方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("No");
            }

        }

      
    }
}
