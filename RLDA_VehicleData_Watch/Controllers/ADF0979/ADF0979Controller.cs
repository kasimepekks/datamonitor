using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using RLDA_VehicleData_Watch.Models;

namespace RLDA_VehicleData_Watch.Controllers
{
    /// <summary>
    /// 此控制器主要功能为生成一些view
    /// </summary>
    [Authorize]
    public class ADF0979Controller : Controller
    {
        private readonly ILogger<ADF0979Controller> _logger;
        public ADF0979Controller(ILogger<ADF0979Controller> logger)
        {
            this._logger = logger;
           
        }

        public IActionResult DisPlayAll()
        {
            //if (!MyQuartzScheduler.TaskAlreadyRun)
            //{
            //    await MyQuartzScheduler.RunMonitor();
            //}

            
            _logger.LogInformation("启动了监控页面");
            return View();
        }
        /// <summary>
        /// 返回添加ACC数据的页面
        /// </summary>
        /// <returns></returns>
        public IActionResult AddDistance()
        {
            _logger.LogInformation("启动了处理ACC里程的页面");
            return View();
        }
        /// <summary>
        ///返回添加WFT数据的页面
        /// </summary>
        /// <returns></returns>
        public IActionResult AddWFT()
        {
            _logger.LogInformation("启动了处理WFT数据的页面");
            return View();
        }
        public IActionResult DataAnalysis()
        {
            _logger.LogInformation("启动了数据分析的页面");
            return View();
        }
        public IActionResult DataPrediction()
        {
            _logger.LogInformation("启动了损伤预测的页面");
            return View();
        }
        public IActionResult CoravelJob()
        {
            _logger.LogInformation("启动了定时处理数据的页面");
            return View();
        }

      
    }
}
