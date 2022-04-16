using BLL.SH_ADF0979BLL;
using DAL.SH_ADF0979;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Pomelo.AspNetCore.TimedJob;
using Microsoft.Extensions.Configuration;
using RLDA_VehicleData_Watch.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;
using Microsoft.Extensions.Logging;

namespace RLDA_VehicleData_Watch.Models
{
    /// <summary>
    /// 轻量级定时任务，不需要实例化即可开启定时
    /// </summary>
    //public class MyTimeJob : Job
    //{
    //    private readonly IConfiguration _configuration;
    //    private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
    //    private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
    //    private readonly ILogger<MyTimeJob> _logger;
   

    //     public MyTimeJob( IConfiguration configuration, ILogger<MyTimeJob> logger)
    //     {
    //        _configuration = configuration;
    //        _logger = logger;
    //     }

    //    //    //SkipWhileExecuting = true必须做完定时任务再开启下个定时
    //    //    //程序一开始就会启动，所以会先扫描文件夹，如没有则会新建文件夹
    //    [Invoke(Interval = 1000 * 10, SkipWhileExecuting = true)]
    //    public void TimeJob()
    //    {

            //        //var inputpath=_configuration["FilePath:inputpath"];
            //        //var resultpath = _configuration["FilePath:result"];
            //        //返回最新的csv文件的绝对路径，创建时间和文件名
            //        FileTimeInfo inputfiletimeinfo = FileOperator.GetLatestFileTimeInfo(inputpath+datefile, ".csv");
            //        FileTimeInfo outputfiletimeinfo = FileOperator.GetLatestFileTimeInfo(resultpath+datefile, ".csv");
            //        Task.Run(() =>
            //        {
            //            if (inputfiletimeinfo != null && inputfiletimeinfo.FileName != inputlastfile.FileName)
            //            {
            //                //double sdistance;
            //                //读取input文件夹下的最新的csv文件并导入到数据库中，导入之前删除数据库表中的上一个文件的数据
            //                var accstatisticresult = _IStatistic_ACC_Service.ReadOneCsvFileForStatisticService(inputfiletimeinfo.FullFileName, inputfiletimeinfo.FileName, out name, out sdistance);
            //                //var acctimedomainresult = _IRealTime_ACC_Service.ReadOneFileForRealTimeAccReturn(inputfiletimeinfo.FullFileName, inputfiletimeinfo.FileName,out speed, out brake);
            //                var acctimedomainresult = _IRealTime_ACC_Service.ReadOneCsvFileService(inputfiletimeinfo.FullFileName, inputfiletimeinfo.FileName, out name, out speed, out brake,out Lat,out Lon);

            //                double dis = Math.Round(sdistance, 2);
            //                distance += dis;
            //                int zerotime = 0;
            //                //speed = _IRealTime_ACC_Service.GetRealTimeSpeedLsit();
            //                //brake = _IRealTime_ACC_Service.GetRealTimeBrakeLsit();
            //                //_httpContextAccessor.HttpContext.Session.SetInt32("Distance", distance);//设置distance的session
            //                inputlastfile = inputfiletimeinfo;
            //                //向所有登录用户发送信息，前端可以接受此信息并作出响应
            //                //_hubContext.Clients.All.SendAsync("ReloadData");
            //                _hubContext.Clients.All.SendAsync("ReloadDataACC", name, acctimedomainresult, accstatisticresult);
            //                _hubContext.Clients.All.SendAsync("SpeedtoDistance", distance, speed, brake,Lat,Lon,zerotime);//zerotime用来初始化每次的开始时间，每当有新数据读取时，zerotime初始化为0，传入前端，用于前端的speed和brake仪表盘的显示

            //                //读取output文件夹下的数据

            //            }
            //        });
            //        Task.Run(() =>
            //        {
            //            if (outputfiletimeinfo != null && outputfiletimeinfo.FileName != outputlastfile.FileName)
            //            {
            //                //_IStatistic_WFT_Service.ReadOneFileForStatisticWFT(outputfiletimeinfo.FullFileName, outputfiletimeinfo.FileName);
            //                //_IRealTime_WFT_Service.ReadOneFileForRealTimeWFT(outputfiletimeinfo.FullFileName, outputfiletimeinfo.FileName);
            //                //var timedomainresult = _IRealTime_WFT_Service.ReadOneFileForRealTimeWFTReturn(outputfiletimeinfo.FullFileName, outputfiletimeinfo.FileName);
            //                var timedomainresult = _IRealTime_WFT_Service.ReadOneCsvFileService(outputfiletimeinfo.FullFileName, outputfiletimeinfo.FileName, out name, out speed, out brake,out Lat,out Lon);

            //                var statisticresult = _IStatistic_WFT_Service.ReadOneCsvFileForStatisticService(outputfiletimeinfo.FullFileName, outputfiletimeinfo.FileName, out name, out sdistance);

            //                outputlastfile = outputfiletimeinfo;
            //                //_hubContext.Clients.All.SendAsync("ReloadDataWFT");
            //                _hubContext.Clients.All.SendAsync("ReloadDataWFT", name, timedomainresult, statisticresult);
            //            }
            //        });

            //    }



        }
            
    //}
//}
