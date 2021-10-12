using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MysqlforDataWatch;
using Tools;
using Tools.AddDistance;

namespace RLDA_VehicleData_Watch.Controllers
{
    /// <summary>
    /// 此控制器主要功能为添加里程分布和统计加速度和测量轮最大最小rms等参数，自己在前端选择日期时间进行计算
    /// </summary>
    public class AnalysisController : Controller
    {
        private readonly datawatchContext _DB;
        private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly ISpeedDistribution_ACC_IBLL _ISpeedDistribution_ACC_Service;
        private readonly IConfiguration _configuration;
        private readonly string inputpath;
        private readonly string resultpath;
        //private double Distance = 0;
        public AnalysisController(IAnalysisData_ACC_IBLL IAnalysisData_ACC_Service, IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, ISpeedDistribution_ACC_IBLL ISpeedDistribution_ACC_Service, IConfiguration configuration, datawatchContext db)
        {
            _IAnalysisData_ACC_Service = IAnalysisData_ACC_Service;
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            _ISpeedDistribution_ACC_Service = ISpeedDistribution_ACC_Service;
            _configuration = configuration;
            _DB = db;
            inputpath = _configuration["DataMonitor:inputpath"];
            resultpath = _configuration["DataMonitor:resultpath"];
        }
        /// <summary>
        /// 前端ajxs请求进来后执行添加distance字段并进行统计后存入数据库中,里程分布统计操作
        /// </summary>
        /// <returns></returns>
        public IActionResult AddDistanceandsavetoSql(string datetime)
        {
            try
            {
                if (datetime != null)
                {
                    string date = FileOperator.DatetoName(datetime.Substring(5));//把2020-07-01日期格式改为07_01
                    string inputfiletimeinfo = inputpath + date;
                    if (_IAnalysisData_ACC_Service.ReadandMergeACCDataperHalfHour(inputfiletimeinfo))
                    {
                        //_ISpeedDistribution_ACC_Service.ReadandMergeSpeedDistributionAcc(inputfiletimeinfo);//统计speed分布，并存入数据库
                        return Content("执行完毕！");
                        //if (!AddDistanceforAnalysisData.AddDistanceforAnalysisDataAcc(inputfiletimeinfo))//先添加distance列到每一个csv文件
                        //{

                        //    _ISpeedDistribution_ACC_Service.ReadandMergeSpeedDistributionAcc(inputfiletimeinfo);//统计speed分布，并存入数据库
                        //    return Content("执行完毕！");
                        //}
                        //else
                        //{
                        //    return Content("已执行过！");//不可能执行，因为判断条件一直为真，即使这样也没有关系
                        //}
                    }
                    else
                    {
                        return Content("无数据文件");
                    }

                }

                return Content("请选择日期！");
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
           
           
        }
        /// <summary>
        /// 执行result文件的计算和导入数据库
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public IActionResult AddWFTtoSql(string datetime)
        {
            try
            {
                if (datetime != null)
                {
                    string date = FileOperator.DatetoName(datetime.Substring(5));

                    string outputfiletimeinfo = resultpath + date;


                    if (_IAnalysisData_WFT_Service.ReadandMergeWFTDataperHalfHour(outputfiletimeinfo))
                    {
                        return Content("执行完毕！");

                    }

                    else
                    {
                        return Content("无数据文件");
                    }



                }

                return Content("请选择日期！");
            }
            catch (Exception ex)
            {

                throw ex;
            }
           

        }
        /// <summary>
        /// 根据所选的时间范围来传给前端速度的总体分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public IActionResult SpeedAnalysis(string startdate,string enddate)
        {
            var sd = Convert.ToDateTime(startdate);
            var ed = Convert.ToDateTime(enddate);
            var SpeedList=_ISpeedDistribution_ACC_Service.LoadSpeedDistribution(sd, ed);
            var json= Json(SpeedList);//这里speedlist是iquerable匿名类型，不知为什么无法传给前端layui表格，所以只能直接传json数据
            return json;
        }
        /// <summary>
        /// 根据所选的时间范围来传给前端每天的里程分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public IActionResult SpeedPerDayAnalysis(string startdate, string enddate)
        {
            var sd = Convert.ToDateTime(startdate);
            var ed = Convert.ToDateTime(enddate);
            var SpeedPerDayList = _ISpeedDistribution_ACC_Service.LoadSpeedDistributionperday(sd, ed);
            return Json(SpeedPerDayList);
        }
        /// <summary>
        /// 根据所选的时间范围来传给前端每时的里程分布
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public IActionResult SpeedPerHourAnalysis(string startdate, string enddate)
        {
            var sd = Convert.ToDateTime(startdate);
            var ed = Convert.ToDateTime(enddate);
            var SpeedPerHourList = _ISpeedDistribution_ACC_Service.LoadSpeedDistributionperhour(sd, ed);
            return Json(SpeedPerHourList);
        }

        public IActionResult WFTDamageAnalysis(string startdate, string enddate)
        {
            var sd = Convert.ToDateTime(startdate);
            var ed = Convert.ToDateTime(enddate);
            var damagelist = _IAnalysisData_WFT_Service.LoadWFTDamage(sd, ed);
            return Json(damagelist);
        }
    }
}
