using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RLDA_VehicleData_Watch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RLDA_VehicleData_Watch.Controllers.Base
{
    public class FileWatchController : Controller
    {
        //private readonly IStatistic_ACC_IBLL _IStatistic_ACC_Service;
        private readonly IRealTime_ACC_IBLL _IRealTime_ACC_Service;
        //private readonly IStatistic_WFT_IBLL _IStatistic_WFT_Service;
        private readonly IRealTime_WFT_IBLL _IRealTime_WFT_Service;
        private readonly IHubContext<MyHub> _hubContext;//signalr
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileWatchController> _logger;
        private static string filewatcherpath;
        private static string filewatcherneed;

        
        private string vehicleid;
        

        private static Dictionary<string, double> vehicledistance = new Dictionary<string, double>();//存储每个车的里程
        private static Dictionary<string, double> vehiclecumdistance = new Dictionary<string, double>();//存储每个车的里程
        private static Dictionary<string, string> filename = new Dictionary<string, string>();//存储每个文件名


        public FileWatchController( IRealTime_ACC_IBLL IRealTime_ACC_Service, IRealTime_WFT_IBLL IRealTime_WFT_Service, IHubContext<MyHub> hubContext, IConfiguration configuration,ILogger<FileWatchController> logger)
        {
            //_IStatistic_ACC_Service = IStatistic_ACC_Service;
            _IRealTime_ACC_Service = IRealTime_ACC_Service;
            //_IStatistic_WFT_Service = IStatistic_WFT_Service;
            _IRealTime_WFT_Service = IRealTime_WFT_Service;
            _configuration = configuration;
            this._logger = logger;
            _hubContext = hubContext;
            filewatcherneed = _configuration["DataMonitor:MonitorRequired"];
        }

        public IActionResult FileWatch(string _vehicleID)
        {
            if (filewatcherneed == "false")
            {
                filewatcherpath = _configuration[_vehicleID + ":filewatcherpath"];

                _logger.LogInformation("启动了" + _vehicleID + "的监控页面" + filewatcherpath);
                vehicleid = _vehicleID;

                if (vehiclecumdistance.Count != 0)
                {
                    if (!vehiclecumdistance.ContainsKey(_vehicleID))
                    {
                        vehiclecumdistance.Add(_vehicleID, 0);

                    }
                }
                else
                {
                    vehiclecumdistance.Add(_vehicleID, 0);

                }

                if (vehicledistance.Count != 0)
                {
                    if (!vehicledistance.ContainsKey(_vehicleID))
                    {
                        vehicledistance.Add(_vehicleID, 0);

                    }

                }
                else
                {
                    vehicledistance.Add(_vehicleID, 0);

                }

                if (filename.Count != 0)
                {
                    if (!filename.ContainsKey("input"))
                    {
                        filename.Add("input", "");

                    }
                    else if (!filename.ContainsKey("result"))
                    {
                        filename.Add("result", "");
                    }
                }
                else
                {
                    filename.Add("input", "");
                    filename.Add("result", "");

                }
                WatcherStrat(filewatcherpath, "*.csv");
                return Ok();
            }
            else
            {
                return Ok();
            }
        }


        public  void WatcherStrat(string path, string filter)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = filter;
            watcher.IncludeSubdirectories = true;
            //watcher.NotifyFilter = NotifyFilters.Size;
            watcher.Created += new FileSystemEventHandler(OnProcess);
            //watcher.Changed += new FileSystemEventHandler(OnProcess);
            watcher.EnableRaisingEvents = true;
            
        }

        private  void OnProcess(object source, FileSystemEventArgs e)
        {


            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                Task.Run(()=>OnCreated(source, e));
            }


        }
        private void Waiting(string path)

        {

            try
            {
                FileInfo fi;
                fi = new FileInfo(path);
                long len1, len2;
                len2 = fi.Length;
                do
                {
                    len1 = len2;
                    Thread.Sleep(100);//等待1秒钟
                    fi.Refresh();//这个语句不能漏了
                    len2 = fi.Length;
                } while (len1 < len2);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
        }
    

        private  async Task OnCreated(object source, FileSystemEventArgs e)
        {
                try
                {
                //保证不会重复读取同一个文件
                if (e.FullPath.Contains("input") && filename["input"] != e.Name) {
                    filename["input"] = e.Name;
                    Waiting(e.FullPath);
                   
                    _logger.LogInformation(e.Name + "已被监控发现");
                   await Task.Run(async() =>
                    {
                        
                        var structall =await  _IRealTime_ACC_Service.ReadCSVFileAll(e.FullPath, e.Name);
                        vehicledistance[vehicleid] = Math.Round(structall.sdistance, 2);
                        vehiclecumdistance[vehicleid] += vehicledistance[vehicleid];
                        int zerotime = 0;
                        foreach (var i in MyHub.user)
                        {
                            await _hubContext.Clients.Group(i).SendAsync("SpeedtoDistance", vehicleid, vehiclecumdistance[vehicleid], structall.Speed, structall.Brake, structall.Lat, structall.Lon, structall.StrgWhlAng, zerotime);//zerotime用来初始化每次的开始时间，每当有新数据读取时，zerotime初始化为0，传入前端，用于前端的speed和brake仪表盘的显示\
                            await _hubContext.Clients.Group(i).SendAsync("ReloadDataACC", vehicleid, structall.name, structall.TListReSampling, structall.STList);

                        }

                        _logger.LogInformation(filename["input"] + "已传送数据");
                    });

                }                 

                else if (e.FullPath.Contains("result") && filename["result"] != e.Name)
                {
                    filename["result"] = e.Name;
                    Waiting(e.FullPath);
                    await Task.Run(async () =>
                    {
                        var structresultall = await _IRealTime_WFT_Service.ReadCSVFileAll(e.FullPath, e.Name);
                        foreach(var i in MyHub.user)
                        {
                           await _hubContext.Clients.Group(i).SendAsync("ReloadDataWFT", vehicleid, structresultall.name, structresultall.TListReSampling, structresultall.STList);

                        }
                        _logger.LogInformation(filename["result"] + "已传送数据");
                    });

                }

                }
                catch (Exception ex)
                {

                    _logger.LogInformation(ex.Message);
                }
            
       
           
            


        }
    }
}
