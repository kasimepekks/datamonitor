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
    public class DeleteController : Controller
    {
        //private readonly datawatchContext _DB;
        //private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly ILogger<DeleteController> _logger;
        private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly IBrakeDistribution_IBLL _IBrakeDistribution_Service;
        private readonly IBumpDistribution_IBLL _IBumpDistribution_Service;

        private readonly ISpeedDistribution_ACC_IBLL _ISpeedDistribution_ACC_Service;
        private readonly IThrottleDistribution_IBLL _IThrottleDistribution_Service;
        private readonly ISteeringDistribution_IBLL _ISteeringDistribution_Service;

        private readonly IGPSRecord_IBLL _IGPSRecord_Service;

        //private readonly IConfiguration _configuration;
        //private readonly string AnalysisRequired;
        //private readonly string AnalysisVehicleID;
        //private readonly int reducetimeforgps;
        //private double Distance = 0;
        public DeleteController(IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, IAnalysisData_ACC_IBLL IAnalysisData_ACC_Service, ISpeedDistribution_ACC_IBLL ISpeedDistribution_ACC_Service,IBrakeDistribution_IBLL IBrakeDistribution_Service, IBumpDistribution_IBLL IBumpDistribution_Service, ISteeringDistribution_IBLL ISteeringDistribution_Service, IGPSRecord_IBLL IGPSRecord_Service,  IThrottleDistribution_IBLL IThrottleDistribution_Service, ILogger<DeleteController> logger)
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
            //_configuration = configuration;
            this._logger = logger;
            //_DB = db;
           
        }
        /// <summary>
        /// 删除指定日期的数据
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="vehicleid"></param>
        /// <returns></returns>
        public IActionResult AllDataDelete(string startdate, string enddate, string vehicleid)
        {
            try
            {

                var sd = Convert.ToDateTime(startdate);
                var ed = Convert.ToDateTime(enddate);
                var speeddelete = _ISpeedDistribution_ACC_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                var accdelete = _IAnalysisData_ACC_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                var wftdelete = _IAnalysisData_WFT_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                var brakedelete = _IBrakeDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                var bumpdelete = _IBumpDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                var throttledelete = _IThrottleDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                var steerdelete = _ISteeringDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                var gpsdelete = _IGPSRecord_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);

                return Content("删除成功! ");
            }
            catch (Exception ex)
            {

                _logger.LogInformation("DeleteController中DataDelete方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("No");
            }

        }

        public IActionResult DataSingleDelete(string startdate, string enddate, string vehicleid,string formvalue)
        {
            try
            {

                var sd = Convert.ToDateTime(startdate);
                var ed = Convert.ToDateTime(enddate);
                if (formvalue == "0")
                {
                    var wftdelete = _IAnalysisData_WFT_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);

                }
                else if(formvalue == "1")
                {
                    var accdelete = _IAnalysisData_ACC_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);

                }
                else if (formvalue == "2")
                {
                    var speeddelete = _ISpeedDistribution_ACC_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);

                }
                else if (formvalue == "3")
                {
                    var bumpdelete = _IBumpDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);

                }
                else if (formvalue == "4")
                {
                    var brakedelete = _IBrakeDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);

                }
                //var throttledelete = _IThrottleDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                //var steerdelete = _ISteeringDistribution_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);
                //var gpsdelete = _IGPSRecord_Service.DeleteAllEntity(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid);

                return Content("删除成功! ");
            }
            catch (Exception ex)
            {

                _logger.LogInformation("DeleteController中DataSingleDelete方法出现问题：" + ex.Message + "出现时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return Json("No");
            }

        }
    }
}
