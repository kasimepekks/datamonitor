using Coravel.Invocable;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;

namespace RLDA_VehicleData_Watch.Models
{
    //这里每次定时都会重新构造一次
    public class MyInvocable : IInvocable
    {
        
        private readonly IRealTime_ACC_IBLL _IRealTime_ACC_Service;
        
        private readonly IRealTime_WFT_IBLL _IRealTime_WFT_Service;
        private readonly IHubContext<MyHub> _hubContext;//signalr
        private readonly IConfiguration _configuration;
        
        private readonly string inputpath;
        private readonly string resultpath;
        private readonly string _vehicleID;
        private readonly string _MonitorvehicleID;//配置文件里的需要监控的车辆
        private static Dictionary<string, FileTimeInfo> newdictionary = new Dictionary<string, FileTimeInfo>();//存储临时的最新文件，与vehicledictionary进行判断，也就是与下一个最新文件进行判断，如果相同，则不是最新的文件
        private static Dictionary<string, string> pathdictionary = new Dictionary<string, string>();//存储每个车辆的读取文件的源路径
        private static Dictionary<string, FileTimeInfo> vehicledictionary = new Dictionary<string, FileTimeInfo>();//存储按照车辆编号的最新文件
        private static Dictionary<string, double> vehicledistance = new Dictionary<string, double>();//存储每个车的里程
        private static Dictionary<string, double> vehiclecumdistance = new Dictionary<string, double>();//存储每个车的里程


        readonly string datefile = FileOperator.DatetoName(DateTime.Now.ToString("MM-dd"));//日期文件夹名
        public MyInvocable(IRealTime_ACC_IBLL IRealTime_ACC_Service,  IRealTime_WFT_IBLL IRealTime_WFT_Service, IHubContext<MyHub> hubContext, IConfiguration configuration, string vehicleID)
        {
            
            _IRealTime_ACC_Service = IRealTime_ACC_Service;
            
            _IRealTime_WFT_Service = IRealTime_WFT_Service;
            _configuration = configuration;
            _hubContext = hubContext;
            _vehicleID = vehicleID;

            inputpath = _configuration[_vehicleID + ":inputpath"];
            resultpath = _configuration[_vehicleID + ":resultpath"];


            _MonitorvehicleID = _configuration["DataMonitor:MonitoringVehicleID"];

            if (pathdictionary.Count != 0)
            {
                if (!pathdictionary.ContainsKey(_vehicleID + "inputpath"))
                {
                    pathdictionary.Add(_vehicleID + "inputpath", inputpath);
                    pathdictionary.Add(_vehicleID + "outputpath", resultpath);
                }
            }
            else
            {
                pathdictionary.Add(_vehicleID + "inputpath", inputpath);
                pathdictionary.Add(_vehicleID + "outputpath", resultpath);
            }
            if (vehicledictionary.Count != 0)
            {
                if (!vehicledictionary.ContainsKey(_vehicleID + "inputpath"))
                {
                    vehicledictionary.Add(_vehicleID + "inputpath", FileOperator.GetLatestFileTimeInfo(pathdictionary[_vehicleID + "inputpath"] + datefile, ".csv"));

                }
                else
                {
                    vehicledictionary[_vehicleID + "inputpath"] = FileOperator.GetLatestFileTimeInfo(pathdictionary[_vehicleID + "inputpath"] + datefile, ".csv");

                }
                if (!vehicledictionary.ContainsKey(_vehicleID + "outputpath"))
                {
                    vehicledictionary.Add(_vehicleID + "outputpath", FileOperator.GetLatestFileTimeInfo(pathdictionary[_vehicleID + "outputpath"] + datefile, ".csv"));

                }
                else
                {
                    vehicledictionary[_vehicleID + "outputpath"] = FileOperator.GetLatestFileTimeInfo(pathdictionary[_vehicleID + "outputpath"] + datefile, ".csv");

                }
            }
            else
            {
                vehicledictionary.Add(_vehicleID + "inputpath", FileOperator.GetLatestFileTimeInfo(pathdictionary[_vehicleID + "inputpath"] + datefile, ".csv"));
                vehicledictionary.Add(_vehicleID + "outputpath", FileOperator.GetLatestFileTimeInfo(pathdictionary[_vehicleID + "outputpath"] + datefile, ".csv"));
            }

            if (newdictionary.Count != 0)
            {
                if (!newdictionary.ContainsKey(_vehicleID + "input"))
                {
                    newdictionary.Add(_vehicleID + "input", new FileTimeInfo() { FileName = vehicledictionary[_vehicleID + "inputpath"].FileName });

                }
                if (!newdictionary.ContainsKey(_vehicleID + "output"))
                {
                    newdictionary.Add(_vehicleID + "output", new FileTimeInfo() { FileName = vehicledictionary[_vehicleID + "outputpath"].FileName });
                }
            }
            else
            {
                newdictionary.Add(_vehicleID + "input", new FileTimeInfo() { FileName = vehicledictionary[_vehicleID + "inputpath"].FileName });
                newdictionary.Add(_vehicleID + "output", new FileTimeInfo() { FileName = vehicledictionary[_vehicleID + "outputpath"].FileName });
            }

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

        }
     
        //这里日期选择当前时间，0代表input,1代表output


        public Task Invoke()
        {
            if (_MonitorvehicleID != null)
            {
                if (_MonitorvehicleID.Contains(_vehicleID))//先判断是否需要监控配置文件里需要监控的车辆，如配置文件里没有则不需要进行监控了
                {

                    Task.Run(() =>
                    {
                                               
                        if (vehicledictionary[_vehicleID + "inputpath"].FileName != "default" && vehicledictionary[_vehicleID + "inputpath"].FileName != newdictionary[_vehicleID + "input"].FileName)
                        {
                            var structall= _IRealTime_ACC_Service.ReadCSVFileAll(vehicledictionary[_vehicleID + "inputpath"].FullFileName, vehicledictionary[_vehicleID + "inputpath"].FileName);
                            vehicledistance[_vehicleID] = Math.Round(structall.sdistance, 2);

                            vehiclecumdistance[_vehicleID] += vehicledistance[_vehicleID];
                          
                            int zerotime = 0;
                         
                            newdictionary[_vehicleID + "input"] = vehicledictionary[_vehicleID + "inputpath"];

                            //向所有登录用户发送信息，前端可以接受此信息并作出响应
                            //_hubContext.Clients.All.SendAsync("ReloadData");
                            //_hubContext.Clients.Group(MyHub.user).SendAsync("ReloadDataACC", _vehicleID, structall.name, structall.TList, structall.STList);
                            //_hubContext.Clients.Group(MyHub.user).SendAsync("SpeedtoDistance", _vehicleID, vehiclecumdistance[_vehicleID], structall.Speed, structall.Brake, structall.Lat, structall.Lon, structall.StrgWhlAng, zerotime);//zerotime用来初始化每次的开始时间，每当有新数据读取时，zerotime初始化为0，传入前端，用于前端的speed和brake仪表盘的显示

                            _hubContext.Clients.All.SendAsync("ReloadDataACC", _vehicleID, structall.name, structall.TList, structall.STList);
                            _hubContext.Clients.All.SendAsync("SpeedtoDistance", _vehicleID, vehiclecumdistance[_vehicleID], structall.Speed, structall.Brake, structall.Lat, structall.Lon, structall.StrgWhlAng, zerotime);

                            
                        }

                        if (vehicledictionary[_vehicleID + "outputpath"].FileName != "default" && vehicledictionary[_vehicleID + "outputpath"].FileName != newdictionary[_vehicleID + "output"].FileName)
                        {
                            var structresultall = _IRealTime_WFT_Service.ReadCSVFileAll(vehicledictionary[_vehicleID + "outputpath"].FullFileName, vehicledictionary[_vehicleID + "outputpath"].FileName);
                            newdictionary[_vehicleID + "output"] = vehicledictionary[_vehicleID + "outputpath"];
                           
                            //_hubContext.Clients.Groups(MyHub.user).SendAsync("ReloadDataWFT", _vehicleID, structresultall.name, structresultall.TList, structresultall.STList);
                            _hubContext.Clients.All.SendAsync("ReloadDataWFT", _vehicleID, structresultall.name, structresultall.TList, structresultall.STList);
                        }
                    });

                }
            }


            return Task.CompletedTask;
        }
    }
}
