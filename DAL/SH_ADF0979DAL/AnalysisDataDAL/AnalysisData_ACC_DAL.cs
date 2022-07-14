using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Tools.AddDamage;
using Tools.AddDistance;
using Tools.Cash;
using Tools.GPSCal;
using Tools.ListOperation;
using Tools.ListOperation.BrakeListOperation;
using Tools.ListOperation.BumpListOperation;
using Tools.ListOperation.StatisticAccListOperation;
using Tools.ListOperation.SteeringListOperation;
using Tools.ListOperation.ThrottleListOperation;
using Tools.MyConfig;

namespace DAL.SH_ADF0979DAL
{
  public  class AnalysisData_ACC_DAL : BaseDAL<SatictisAnalysisdataAcc>, IAnalysisData_ACC_IDAL
    {
        public AnalysisData_ACC_DAL(datawatchContext DB) : base(DB)
        {

        }

        /// <summary>
        /// 把每个csv文件计算统计值并存入数据库中，不记录累积里程和单个里程
        /// </summary>
       

        public async Task<bool> ReadandMergeACCDataperHalfHour(string filepath,string vehicleid, VehicleIDPara vehicleIDPara)
        {

            FileInfo[] filelist = FileOperator.Isfileexist(filepath);//获得指定文件下的所有csv文件
            Encoding encoding = Encoding.Default;
            bool can = false;
            await Task.Run(() =>
            {
                if (filelist.Length > 0)
                {
                    List<Speeddistribution> _SpeeddistributionList = new List<Speeddistribution>();
                    List<SatictisAnalysisdataAcc> _SatictisAnalysisdataAccList = new List<SatictisAnalysisdataAcc>();
                    List<Bumprecognition> bumpsqllist = new List<Bumprecognition>();
                    List<Brakerecognition> brakesqllist = new List<Brakerecognition>();
                    List<Streeringrecognition> steeringsqllist = new List<Streeringrecognition>();
                    List<Throttlerecognition> throttlesqllist = new List<Throttlerecognition>();
                    List<Gpsrecord> gpsrecordlist = new List<Gpsrecord>();

                    //以下是获取了数据库每个表的id列，用于判断是否之前已经导入过相同的数据
                    var bumpmysqllist = _DB.Set<Bumprecognition>().Select(a => a.Id).ToList();
                    var brakemysqllist = _DB.Set<Brakerecognition>().Select(a => a.Id).ToList();
                    var steeringmysqllist = _DB.Set<Streeringrecognition>().Select(a => a.Id).ToList();
                    var speedmysqllist = _DB.Set<Speeddistribution>().Select(a => a.Id).ToList();
                    var accmysqllist = _DB.Set<SatictisAnalysisdataAcc>().Select(a => a.Id).ToList();
                    var throttlemysqllist = _DB.Set<Throttlerecognition>().Select(a => a.Id).ToList();
                    var gpssqllist=_DB.Set<Gpsrecord>().Select(a => a.Id).ToList();


                    //PropertyInfo[] props = ReadTimedomainCash<RealtimeTempdataAcc>.GetProps();
                    bool StrgWhlAngExist = true;
                    bool Brakeisexist = true;
                    bool AccelActuExist = true;
                    foreach (var file in filelist)
                    {

                        if (file.Length != 0)
                        {
                            string[] filestring = file.Name.Split('-');//把文件名每隔“-”拿出来放在这个string数组里
                            string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
                            string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
                            string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
                            string newminute = (int.Parse(timestring[1]) / 30 * 30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
                            string newtime = timestring[0] + ":" + newminute + ":" + "00";

                            string datetime = date + " " + newtime;
                            //var datestart= Convert.ToDateTime(datetime);


                            string name = file.Name.Split('.')[0];
                            //自动new配置表里的相同通道数量的List<double>，用于存储csv里的需要的每个通道的值
                            List<List<double>> AllList = new List<List<double>>();
                           
                            List<double> Speed = new List<double>();

                            for (int i = 0; i < vehicleIDPara.channels.Length; i++)
                            {
                                AllList.Add(new List<double>());
                            }

                            //List<double> singledistance = new List<double>();
                            //List<int> WFT_AZ_LFPeakTimeList = new List<int>();
                            //List<int> WFT_AZ_RFPeakTimeList = new List<int>();
                            //List<int> WFT_AZ_LRPeakTimeList = new List<int>();
                            //List<double> WFT_AZ_LFPeakList = new List<double>();
                            //List<double> WFT_AZ_RFPeakList = new List<double>();
                            //List<double> WFT_AZ_LRPeakList = new List<double>();
                            //List<double> speedpeaklist = new List<double>();



                            if (AllList.Count() > 10)
                            {
                                for (int i = 1; i < AllList.Count - 2; i++)
                                {
                                    AllList[i].Add(0);//所有通道，除了最后2个gps和第一个time，每次都在开始加一个0数据
                                }
                                
                            }


                            FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                            StreamReader sr = new StreamReader(fs, encoding);
                            DataTable dt = new DataTable();
                            string strLine = "";
                            //记录每行记录中的各字段内容
                            string[] aryLine = null;
                            string[] tableHead = null;

                            int resample = 0;//设置一个计数
                            //标示列数
                            int columnCount = 0;
                            //标示是否是读取的第一行
                            bool IsFirst = true;
                            //逐行读取CSV中的数
                            while ((strLine = sr.ReadLine()) != null)
                            {
                               
                                if (IsFirst == true)
                                {
                                    tableHead = strLine.Split(',');
                                    IsFirst = false;
                                    columnCount = tableHead.Length;
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        var t = tableHead[i].Replace("_", "");
                                        tableHead[i] = t.Replace(" ", "");
                                    }

                                    if (!tableHead.Contains("StrgWhlAng"))
                                    {
                                        StrgWhlAngExist = false;
                                    }
                                    //csv文件里有没有这2个列名，如果都没有则设为false
                                    if (!(tableHead.Contains("Brake") || tableHead.Contains("BrkPdlDrvrAp")))
                                    {
                                        Brakeisexist = false;
                                    }
                                    if (!tableHead.Contains("EPTAccelActu"))
                                    {
                                        AccelActuExist = false;
                                    }

                                    //创建列
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        DataColumn dc = new DataColumn(tableHead[i]);
                                        dt.Columns.Add(dc);
                                    }
                                }
                                else
                                {
                                    resample += 1;//每读取一行就加1，直到加到采样率标准就读取数据
                                    //判断是否等于采样率标准
                                    if (resample== vehicleIDPara.Reductiontimesforimport)
                                    {
                                        aryLine = strLine.Split(',');

                                        for (int i = 0; i < vehicleIDPara.channels.Length; i++)
                                        {
                                            if (tableHead.ToList().IndexOf(vehicleIDPara.channels[i]) != -1)
                                            {
                                                //TryParse方法string转double，不管string是什么都能转，不会出错，但是不是数字会转成NaN，所以后面要加判断
                                                double.TryParse(aryLine[tableHead.ToList().IndexOf(vehicleIDPara.channels[i])], out double number);
                                                if (!double.IsNaN(number))
                                                {

                                                    if (vehicleIDPara.channels[i] == ("VehSpdAvg"))//会有小于0的数据，所以全部更改为0
                                                    {

                                                        AllList[i].Add(number < 0 ? 0:number) ;
                                                        Speed.Add(number < 0 ? 0 : number);
                                                    }
                                                    else if(vehicleIDPara.channels[i] == ("StrgWhlAng"))
                                                    {
                                                        AllList[i].Add(number < -600 ? 0 : number);//会有小于-600的转向角度
                                                    }
                                                    else
                                                    {
                                                        AllList[i].Add(number);
                                                    }
                                                   
                                                }
                                                else
                                                {
                                                    AllList[i].Add(0);
                                                }


                                            }
                                            else
                                            {
                                                return;//如果有不匹配的通道名，直接跳出方法
                                            }
                                            

                                        }

                                        
                                        //由于统计值是用的datarow，不是用alllist，所以这里也要把StrgWhlAng作预处理
                                        DataRow dr = dt.NewRow();
                                        for (int j = 0; j < columnCount; j++)
                                        {
                                           
                                            var transfersuceess=double.TryParse(aryLine[j], out double value);
                                            if (transfersuceess)
                                            {
                                                if (!double.IsNaN(value))
                                                {
                                                    if (tableHead[j] == "StrgWhlAng")
                                                    {
                                                        dr[j] = value < -600 ? "0" : aryLine[j];
                                                    }

                                                    else if (tableHead[j] == "Spd")
                                                    {
                                                        dr[j] = value > 200 ? "0" : aryLine[j];
                                                    }
                                                    else if (tableHead[j] == "VehSpdAvg")
                                                    {
                                                        dr[j] = value < 0 ? "0" : aryLine[j];
                                                    }
                                                    else
                                                    {
                                                        dr[j] = value;
                                                    }

                                                }
                                                else
                                                {
                                                    dr[j] = "0";
                                                }
                                            }
                                            else
                                            {
                                                dr[j] = "0";
                                            }  
                                           
                                          
                                        }
                                        dt.Rows.Add(dr);
                                        resample = 0;//重新计数
                                    }

                                }
                            }



                            if (AllList.Count() > 10)
                            {
                                for (int i = 1; i < AllList.Count - 2; i++)
                                {
                                    AllList[i].Add(0);//所有通道除了，除了最后2个gps和第一个time，每次都在最后加一个0数据
                                }

                            }


                            sr.Close();
                            sr.Dispose();

                            fs.Close();
                            fs.Dispose();
                            if (aryLine != null && aryLine.Length > 0)
                            {
                                try
                                {
                                    //筛选GPS信号并存储到数据库中

                                    if (vehicleIDPara.GPSImport == "true")
                                    {
                                        var gpslist = AddGPS.addgpslist(AllList[11], AllList[12], Speed, gpssqllist, vehicleid, name, datetime, vehicleIDPara);
                                        if (gpslist.Count > 0)
                                        {
                                            gpsrecordlist = gpsrecordlist.Concat(gpslist).ToList();

                                        }
                                    }


                                    //计算统计值并存储到数据库中
                                    if (vehicleIDPara.StatisticImport == "true")
                                    {
                                        var statisticlist = AddStatisticacc.addstatisticlist(columnCount, tableHead, dt, vehicleid, name, datetime, vehicleIDPara);
                                        if (statisticlist.Count > 0)
                                        {
                                            _SatictisAnalysisdataAccList = _SatictisAnalysisdataAccList.Concat(statisticlist).ToList();
                                        }
                                    }



                                    //计算速度分布并存储到数据库中
                                    if (vehicleIDPara.SpeedImport == "true")
                                    {
                                        var speedlist = AddSpeedDistribution.addspeedlist(Speed, AllList[0], vehicleid, name, datetime, vehicleIDPara);
                                        if (speedlist.Count > 0)
                                        {
                                            _SpeeddistributionList = _SpeeddistributionList.Concat(speedlist).ToList();
                                        }
                                    }



                                    //计算冲击工况并存储到数据库中
                                    if (vehicleIDPara.BumpImport == "true")
                                    {
                                        var bumplist = AddBump.addbumplist(AllList[2], AllList[3], AllList[4], AllList[1], bumpmysqllist, vehicleid, name, datetime, vehicleIDPara);
                                        if (bumplist.Count > 0)
                                        {
                                            bumpsqllist = bumpsqllist.Concat(bumplist).ToList();
                                        }
                                    }


                                    //计算刹车工况并存储到数据库中
                                    if (vehicleIDPara.BrakeImport == "true")
                                    {
                                        var brakelist = AddBrake.addbrakelist(AllList[7], AllList[5], AllList[1], brakemysqllist, vehicleid, name, datetime, Brakeisexist, vehicleIDPara);
                                        if (brakelist.Count > 0)
                                        {
                                            brakesqllist = brakesqllist.Concat(brakelist).ToList();
                                        }
                                    }

                                    //计算转向工况并存储到数据库中
                                    if (vehicleIDPara.SteeringImport == "true")
                                    {
                                        var steeringlist = AddSteering.addsteeringlist(AllList[9], AllList[10], AllList[1], AllList[6], steeringmysqllist, vehicleid, name, datetime, StrgWhlAngExist, vehicleIDPara);
                                        if (steeringlist.Count > 0)
                                        {
                                            steeringsqllist = steeringsqllist.Concat(steeringlist).ToList();
                                        }
                                    }

                                    //计算油门工况并存储到数据库中
                                    if (vehicleIDPara.ThrottleImport == "true")
                                    {
                                        var throttlelist = AddThrottle.addthrottlelist(AllList[8], AllList[5], AllList[1], throttlemysqllist, vehicleid, name, datetime, AccelActuExist, vehicleIDPara);
                                        if (throttlelist.Count > 0)
                                        {
                                            throttlesqllist = throttlesqllist.Concat(throttlelist).ToList();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {

                                    Console.WriteLine("文件名为" + file + "导入时发生错误：" + ex.Message);
                                }

                               
                            }

                        }

                        else
                        {
                            continue;
                        }



                    }


                    var statisticfinallist=WFTCombined.statisticcombine(_SatictisAnalysisdataAccList, accmysqllist, vehicleid);

                    var speedfinallist = SpeedCombined.speedcombine(_SpeeddistributionList, speedmysqllist, vehicleid);

                    if (vehicleIDPara.GPSImport == "true")
                    {
                        if (gpsrecordlist.Count>0)
                        {
                            
                            _DB.BulkInsert(gpsrecordlist);
                        }
                        Console.WriteLine("GPS数据有" + gpsrecordlist.Count + "条");
                    }
                       
                    else
                    {
                        Console.WriteLine("GPS没有开启导入");
                    }

                    if (vehicleIDPara.BumpImport == "true")
                    {
                        if (bumpsqllist.Count>0)
                        {
                            
                            _DB.BulkInsert(bumpsqllist);
                        }
                        Console.WriteLine("冲击数据有" + bumpsqllist.Count + "条");
                    }
                      
                    else
                    {
                        Console.WriteLine("冲击没有开启导入");
                    }

                    if (vehicleIDPara.BrakeImport == "true")
                    {
                        if (brakesqllist.Count > 0)
                        {
                            
                            _DB.BulkInsert(brakesqllist);
                        }
                        Console.WriteLine("制动数据有" + brakesqllist.Count + "条");
                    }
                       
                    else
                    {
                        Console.WriteLine("制动没有开启导入");
                    }

                    if (vehicleIDPara.SteeringImport == "true")
                    {
                        if (steeringsqllist.Count > 0)
                        {
                           
                            _DB.BulkInsert(steeringsqllist);
                        }
                        Console.WriteLine("转向数据有" + steeringsqllist.Count + "条");
                    }
                      
                    else
                    {
                        Console.WriteLine("转向没有开启导入");
                    }

                    if (vehicleIDPara.ThrottleImport == "true")
                    {
                        if (throttlesqllist.Count > 0)
                        {
                            
                            _DB.BulkInsert(throttlesqllist);
                        }
                        Console.WriteLine("油门数据有" + throttlesqllist.Count + "条");
                    }
                        
                    else
                    {
                        Console.WriteLine("油门没有开启导入");
                    }

                    if (vehicleIDPara.StatisticImport == "true")
                    {
                        if (statisticfinallist.Count > 0)
                        {
                            
                            _DB.BulkInsert(statisticfinallist);
                        }
                        Console.WriteLine("统计加速度数据有" + statisticfinallist.Count + "条");
                    }
                       
                    else
                    {
                        Console.WriteLine("统计加速度没有开启导入");
                    }

                    if (vehicleIDPara.SpeedImport == "true")
                    {
                        if (speedfinallist.Count > 0)
                        {
                            
                            _DB.BulkInsert(speedfinallist);
                        }
                        Console.WriteLine("速度数据有" + speedfinallist.Count + "条");
                    }
                       
                    else
                    {
                        Console.WriteLine("速度没有开启导入");
                    }
 
                    _DB.SaveChanges();
                    can= true;

                }
                
            });
            return can;
        }
    }
}
