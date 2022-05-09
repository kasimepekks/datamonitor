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
                    List<Throttlerecognition> throttlelist = new List<Throttlerecognition>();
                    List<Gpsrecord> gpsrecordlist = new List<Gpsrecord>();

                    //以下是获取了数据库每个表的id列，用于判断是否之前已经导入过相同的数据
                    var bumpmysqllist = _DB.Bumprecognitions.Select(a => a.Id).ToList();
                    var brakemysqllist = _DB.Set<Brakerecognition>().Select(a => a.Id).ToList();
                    var steeringmysqllist = _DB.Set<Streeringrecognition>().Select(a => a.Id).ToList();
                    var speedmysqllist = _DB.Set<Speeddistribution>().Select(a => a.Id).ToList();
                    var accmysqllist = _DB.Set<SatictisAnalysisdataAcc>().Select(a => a.Id).ToList();
                    var throttlemysqllist = _DB.Set<Throttlerecognition>().Select(a => a.Id).ToList();
                    var gpssqllist=_DB.Gpsrecords.Select(a => a.Id).ToList();


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
                            for (int i = 0; i < vehicleIDPara.channels.Length; i++)
                            {
                                AllList.Add(new List<double>());
                            }
                            List<double> singledistance = new List<double>();
                            List<int> WFT_AZ_LFPeakTimeList = new List<int>();
                            List<int> WFT_AZ_RFPeakTimeList = new List<int>();
                            List<int> WFT_AZ_LRPeakTimeList = new List<int>();
                            List<double> WFT_AZ_LFPeakList = new List<double>();
                            List<double> WFT_AZ_RFPeakList = new List<double>();
                            List<double> WFT_AZ_LRPeakList = new List<double>();
                            List<double> speedpeaklist = new List<double>();
                            //{ 
                            //List<double> speedlist = new List<double>();
                            //List<double> timelist = new List<double>();
                            //List<double> WFT_AZ_LFList = new List<double>();
                            //List<double> WFT_AZ_RFList = new List<double>();
                            //List<double> WFT_AZ_LRList = new List<double>();
                            //List<double> brakelist = new List<double>();
                            //List<double> Acc_X_FM_list = new List<double>();
                            //List<double> StrgWhlAngList = new List<double>();
                            //List<double> Acc_Y_FM_list = new List<double>();
                            //List<double> AngularAcc_list = new List<double>();
                            //}


                            if (AllList.Count() > 10)
                            {
                                AllList[7].Add(0);//这是刹车通道，每次都在开始加一个0数据
                                AllList[9].Add(0);//这是转角通道，每次都在开始加一个0数据
                            }
                            //brakelist.Add(0);//每次都在开始加一个0数据
                            //StrgWhlAngList.Add(0);//每次都在开始加一个0数据

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
                                resample++;//每读取一行就加1，直到加到采样率标准就读取数据
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
                                    //判断是否等于采样率标准
                                    if(resample== vehicleIDPara.Reductiontimesforimport)
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
                                                    if (vehicleIDPara.channels[i].Contains("N"))
                                                    {
                                                        AllList[i].Add(number*-1);
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
                                            resample = 0;//重新计数

                                        }

                                        DataRow dr = dt.NewRow();
                                        for (int j = 0; j < columnCount; j++)
                                        {
                                            dr[j] = aryLine[j];
                                        }
                                        dt.Rows.Add(dr);
                                    }

                                    {
                                        //if (tableHead.ToList().IndexOf("Speed") != -1)
                                        //{
                                        //    //判断字符串是否是纯数字
                                        //    if (FileOperator.IsNumber(aryLine[tableHead.ToList().IndexOf("Speed")]))
                                        //    {

                                        //        speedlist.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("Speed")]));

                                        //    }
                                        //    else
                                        //    {
                                        //        speedlist.Add(0);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (tableHead.ToList().IndexOf("Spd") != -1)
                                        //    {

                                        //        if (FileOperator.IsNumber(aryLine[tableHead.ToList().IndexOf("Spd")]))
                                        //        {
                                        //            //判断值是否正常

                                        //            speedlist.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("Spd")]));

                                        //        }
                                        //        else
                                        //        {
                                        //            speedlist.Add(0);
                                        //        }
                                        //    }
                                        //}
                                        //if (tableHead.ToList().IndexOf("Time") != -1)
                                        //{

                                        //    timelist.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("Time")]));
                                        //}
                                        //if (tableHead.ToList().IndexOf("AccZWhlLF") != -1)
                                        //{

                                        //    WFT_AZ_LFList.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccZWhlLF")]));
                                        //}
                                        //else
                                        //{
                                        //    //有N的表示方向反了，要乘以-1
                                        //    if (tableHead.ToList().IndexOf("AccZWhlLFN") != -1)
                                        //    {

                                        //        WFT_AZ_LFList.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccZWhlLFN")]) * -1);
                                        //    }
                                        //}
                                        //if (tableHead.ToList().IndexOf("AccZWhlRF") != -1)
                                        //{

                                        //    WFT_AZ_RFList.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccZWhlRF")]));
                                        //}
                                        //else
                                        //{
                                        //    //有N的表示方向反了，要乘以-1
                                        //    if (tableHead.ToList().IndexOf("AccZWhlRFN") != -1)
                                        //    {

                                        //        WFT_AZ_RFList.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccZWhlRFN")]) * -1);
                                        //    }
                                        //}
                                        //if (tableHead.ToList().IndexOf("AccZWhlLR") != -1)
                                        //{

                                        //    WFT_AZ_LRList.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccZWhlLR")]));
                                        //}
                                        //if (tableHead.ToList().IndexOf("Brake") != -1)
                                        //{

                                        //    brakelist.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("Brake")]));
                                        //}
                                        //else
                                        //{
                                        //    if (tableHead.ToList().IndexOf("BrkPdlDrvrAp") != -1)
                                        //    {
                                        //        if (FileOperator.IsNumber(aryLine[tableHead.ToList().IndexOf("BrkPdlDrvrAp")]))
                                        //        {
                                        //            brakelist.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("BrkPdlDrvrAp")]));
                                        //        }
                                        //        else
                                        //        {
                                        //            brakelist.Add(0);
                                        //        }

                                        //    }
                                        //}
                                        ////车身加速度有些是AccXFM，有些是Acc_X_ST_LF_N
                                        //if (tableHead.ToList().IndexOf("AccXFM") != -1)
                                        //{

                                        //    Acc_X_FM_list.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccXFM")]));
                                        //}
                                        //else
                                        //{
                                        //    if (tableHead.ToList().IndexOf("AccXSTLFN") != -1)
                                        //    {

                                        //        Acc_X_FM_list.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccXSTLFN")]) * -1);
                                        //    }
                                        //}
                                        //if (tableHead.ToList().IndexOf("AccYFM") != -1)
                                        //{

                                        //    Acc_Y_FM_list.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccYFM")]));
                                        //}
                                        //else
                                        //{
                                        //    if (tableHead.ToList().IndexOf("AccYSTLF") != -1)
                                        //    {

                                        //        Acc_Y_FM_list.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("AccYSTLF")]));
                                        //    }
                                        //}
                                        //if (StrgWhlAngExist)
                                        //{
                                        //    if (FileOperator.IsNumber(aryLine[tableHead.ToList().IndexOf("StrgWhlAng")]))
                                        //    {

                                        //        StrgWhlAngList.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("StrgWhlAng")]));

                                        //    }
                                        //    else
                                        //    {
                                        //        StrgWhlAngList.Add(0);
                                        //    }

                                        //    if (tableHead.ToList().IndexOf("StrgWhlAngGr") != -1)
                                        //    {
                                        //        if (FileOperator.IsNumber(aryLine[tableHead.ToList().IndexOf("StrgWhlAngGr")]))
                                        //        {
                                        //            AngularAcc_list.Add(Convert.ToDouble(aryLine[tableHead.ToList().IndexOf("StrgWhlAngGr")]));

                                        //        }
                                        //        else
                                        //        {
                                        //            AngularAcc_list.Add(0);
                                        //        }

                                        //    }
                                        //    else
                                        //    {
                                        //        AngularAcc_list.Add(0);//如果没有角加速度，就全部设为0
                                        //    }
                                        //}
                                    }
                                   
                                }
                            }

                            if (AllList.Count() > 10)
                            {
                                AllList[7].Add(0);//这是刹车通道，每次都在最后再加一个0数据
                                AllList[9].Add(0);//这是转角通道，每次都在最后再加一个0数据
                            }
                            //brakelist.Add(0);//每次都在最后再加一个0数据
                            //StrgWhlAngList.Add(0);//每次都在最后再加一个0数据

                            sr.Close();
                            sr.Dispose();

                            fs.Close();
                            fs.Dispose();
                            if (aryLine != null && aryLine.Length > 0)
                            {
                                //筛选GPS信号并存储到数据库中
                                if (AllList.Count > 11)
                                {
                                    var lat = GPS.GPSResampling(AllList[11]);
                                    var lon = GPS.GPSResampling(AllList[12]);
                                    var speed= GPS.GPSResampling(AllList[1]);
                                    for (int i = 0; i < lat.Count; i++)
                                    {
                                        Gpsrecord gpsrecord = new Gpsrecord();
                                        gpsrecord.Id = vehicleid + "-" + name + "-GPS-" + i.ToString();
                                        gpsrecord.Filename = name;
                                        gpsrecord.VehicleId = vehicleid;
                                        gpsrecord.Datadate= Convert.ToDateTime(datetime);
                                        gpsrecord.Lat = lat[i];
                                        gpsrecord.Lon = lon[i];
                                        gpsrecord.Speed = speed[i];
                                        if (!gpssqllist.Contains(gpsrecord.Id))
                                        {
                                            gpsrecordlist.Add(gpsrecord);
                                        }
                                        else
                                        {
                                            continue;
                                        }

                                       
                                    }
                                    
                                }
                              

                                //计算统计值并存储到数据库中
                                for (int l = 0; l < columnCount - 1; l++)
                                {

                                    SatictisAnalysisdataAcc statisticentity = new SatictisAnalysisdataAcc();
                                    //var idinclude = _DB.Set<ShAdf0979SatictisAnalysisdataAcc>().Select(a => a.Id);
                                    statisticentity.Id = vehicleid + "-" + name + "-ACC-" + l.ToString();
                                    statisticentity.VehicleId = vehicleid;
                                    statisticentity.Filename = name;
                                    statisticentity.Datadate = Convert.ToDateTime(datetime);
                                    statisticentity.Chantitle = tableHead[l + 1];

                                    //注意这里由于计算的列的格式是string类型，用max或min计算会出问题，所以必须先转换再求maxmin


                                    statisticentity.Max = dt.AsEnumerable().Max(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));
                                    statisticentity.Min = dt.AsEnumerable().Min(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));

                                    //List<double> damage = dt.AsEnumerable().Select(a => Convert.ToDouble(a.Field<string>(tableHead[l + 1]))).ToList();
                                    ////entity.Damage = GetDamageFromList.GetDamage(damage);
                                    //entity.Damage = damage.GetDamage();

                                    List<string> lst = (from d in dt.AsEnumerable() select d.Field<string>(tableHead[l + 1])).ToList();
                                    double t = 0;
                                    int n = lst.Count;
                                    foreach (var data in lst)
                                    {

                                        t += Convert.ToDouble(data) * Convert.ToDouble(data);
                                    }


                                    statisticentity.Rms = System.Math.Sqrt(t / n);
                                    //    entity.min = dt.AsEnumerable().Select(t => t.Field<double>(tableHead[l + 1])).Min();
                                    statisticentity.Range = statisticentity.Max - statisticentity.Min;

                                    _SatictisAnalysisdataAccList.Add(statisticentity);

                                }
                                //var accumulateddistance = AddDistanceList.AddDistanceToCSV(speedlist, timelist, out singledistance);
                                //singledistance = AddDistanceList.ReturnSingleDistance(speedlist, timelist);
                                singledistance = AddDistanceList.ReturnSingleDistance(AllList[1], AllList[0]);
                                List<double> speeddistribution = new List<double>();
                                //speeddistribution = SpeedDistribution.CalSpeedDistribution(speedlist, singledistance);
                                speeddistribution = SpeedDistribution.CalSpeedDistribution(AllList[1], singledistance);
                                Speeddistribution entity = new Speeddistribution();
                                entity.Id = vehicleid + "-" + name + "-Distribution";
                                entity.VehicleId = vehicleid;
                                entity.Datadate = Convert.ToDateTime(datetime);

                                //entity.Datatime = TimeSpan.Parse(time);
                                entity._010 = speeddistribution[0];
                                entity._1020 = speeddistribution[1];
                                entity._2030 = speeddistribution[2];
                                entity._3040 = speeddistribution[3];
                                entity._4050 = speeddistribution[4];
                                entity._5060 = speeddistribution[5];
                                entity._6070 = speeddistribution[6];
                                entity._7080 = speeddistribution[7];
                                entity._8090 = speeddistribution[8];
                                entity._90100 = speeddistribution[9];
                                entity._100110 = speeddistribution[10];
                                entity._110120 = speeddistribution[11];
                                entity.Above120 = speeddistribution[12];
                                _SpeeddistributionList.Add(entity);



                                //BumpZero.DoZero(WFT_AZ_LFList, WFT_AZ_RFList, WFT_AZ_LRList, vehicleIDPara);
                                BumpZero.DoZero(AllList[2], AllList[3], AllList[4], vehicleIDPara);

                                //PeakSelect.GetPeak(WFT_AZ_LFList, WFT_AZ_RFList, WFT_AZ_LRList, speedlist, vehicleIDPara, out WFT_AZ_LFPeakList, out WFT_AZ_RFPeakList, out WFT_AZ_LRPeakList, out WFT_AZ_LFPeakTimeList, out WFT_AZ_RFPeakTimeList, out WFT_AZ_LRPeakTimeList, out speedpeaklist);
                                PeakSelect.GetPeak(AllList[2], AllList[3], AllList[4], AllList[1], vehicleIDPara, out WFT_AZ_LFPeakList, out WFT_AZ_RFPeakList, out WFT_AZ_LRPeakList, out WFT_AZ_LFPeakTimeList, out WFT_AZ_RFPeakTimeList, out WFT_AZ_LRPeakTimeList, out speedpeaklist);

                                var bumpnocombinelist = BumpReconize.GetBump(WFT_AZ_LFPeakList, WFT_AZ_RFPeakList, WFT_AZ_LRPeakList, WFT_AZ_LFPeakTimeList, WFT_AZ_RFPeakTimeList, WFT_AZ_LRPeakTimeList, speedpeaklist, AllList[1], vehicleIDPara, out List<int> OuttimeListnocombine);
                                if (bumpnocombinelist.Count > 0)
                                {
                                    var bumpcombinedlist = CombineList.CombineListMethod(bumpnocombinelist, OuttimeListnocombine, vehicleIDPara, out List<int> OuttimeList);

                                    //int n =1;
                                    for (int i = 0; i < bumpcombinedlist.Count; i++)
                                    {
                                        Bumprecognition bump = new Bumprecognition();
                                        bump.Id = vehicleid + "-" + name + "-Bump-" + i;

                                        bump.VehicleId = vehicleid;
                                        bump.Datadate = Convert.ToDateTime(datetime);
                                        bump.Filename = name;
                                        bump.BumpAcc = bumpcombinedlist[i];

                                        //判断是否数据库中已经有导入相同的数据了，如果有，就不把这一条数据导入进去
                                        if (!bumpmysqllist.Contains(bump.Id))
                                        {
                                            bumpsqllist.Add(bump);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }

                                }


                                if (Brakeisexist)
                                {
                                    //BrakeZero.DoZero(brakelist, vehicleIDPara);
                                    BrakeZero.DoZero(AllList[7], vehicleIDPara);
                                    //var brakeacclist = BrakeReconize.GetBrake(brakelist, Acc_X_FM_list, speedlist, vehicleIDPara);
                                    var brakeacclist = BrakeReconize.GetBrake(AllList[7], AllList[5], AllList[1], vehicleIDPara);
                                    if (brakeacclist.Count > 0)
                                    {


                                        for (int i = 0; i < brakeacclist.Count; i++)
                                        {
                                            Brakerecognition brake = new Brakerecognition();
                                            brake.Id = vehicleid + "-" + name + "-Brake-" + i;
                                            brake.VehicleId = vehicleid;
                                            brake.Datadate = Convert.ToDateTime(datetime);
                                            brake.Filename = name;
                                            brake.BrakeAcc = brakeacclist[i];

                                            //判断是否数据库中已经有导入相同的数据了，如果有，就不把这一条数据导入进去
                                            if (!brakemysqllist.Contains(brake.Id))
                                            {
                                                brakesqllist.Add(brake);
                                            }
                                            else
                                            {
                                                continue;
                                            }

                                        }

                                    }
                                }

                                if (StrgWhlAngExist)
                                {
                                    //SteeringZero.DoZero(StrgWhlAngList, vehicleIDPara);
                                    SteeringZero.DoZero(AllList[9], vehicleIDPara);
                                    //var SteeringAccList = SteeringReconize.GetSteering(StrgWhlAngList, AngularAcc_list, speedlist, Acc_Y_FM_list, vehicleIDPara);
                                    var SteeringAccList = SteeringReconize.GetSteering(AllList[9], AllList[10], AllList[1], AllList[6], vehicleIDPara);

                                    if (SteeringAccList.Count > 0)
                                    {


                                        for (int i = 0; i < SteeringAccList.Count; i++)
                                        {
                                            Streeringrecognition steering = new Streeringrecognition();
                                            steering.Id = vehicleid + "-" + name + "-Steering-" + i;
                                            steering.VehicleId = vehicleid;
                                            steering.Datadate = Convert.ToDateTime(datetime);
                                            steering.Filename = name;
                                            steering.SteeringAcc = SteeringAccList[i].SteeringStrenth;
                                            steering.StrgWhlAng = SteeringAccList[i].Angle;
                                            steering.Speed = SteeringAccList[i].Speed;
                                            steering.AngularAcc = SteeringAccList[i].AngularAcc;
                                            //判断转向方向
                                            steering.SteeringDirection = (sbyte)(SteeringAccList[i].Angle > 0 ? 1 : -1);
                                            //判断是否数据库中已经有导入相同的数据了，如果有，就不把这一条数据导入进去
                                            if (!steeringmysqllist.Contains(steering.Id))
                                            {
                                                steeringsqllist.Add(steering);
                                            }
                                            else
                                            {
                                                continue;
                                            }

                                            //steeringsqllist.Add(steering);
                                        }

                                    }
                                }

                                if (AccelActuExist)
                                {
                                    ThrottleZero.DoZero(AllList[8], vehicleIDPara);
                                    var throttleList = ThrottleReconize.GetThrottle(AllList[8], AllList[5], AllList[1], vehicleIDPara);
                                    if (throttleList.Count > 0)
                                    {


                                        for (int i = 0; i < throttleList.Count; i++)
                                        {
                                            Throttlerecognition throttlerecognition = new Throttlerecognition();
                                            throttlerecognition.Id = vehicleid + "-" + name + "-Throttle-" + i;
                                            throttlerecognition.VehicleId = vehicleid;
                                            throttlerecognition.Datadate = Convert.ToDateTime(datetime);
                                            throttlerecognition.Filename = name;
                                            throttlerecognition.Accelerograph = throttleList[i].throttle;
                                            throttlerecognition.LastingTime = throttleList[i].lastingtime;
                                            throttlerecognition.Speed = throttleList[i].Speed;
                                            throttlerecognition.ThrottleAcc = throttleList[i].accxst;
                                            throttlerecognition.Reverse = throttleList[i].direction;
                                            //判断是否数据库中已经有导入相同的数据了，如果有，就不把这一条数据导入进去
                                            if (!throttlemysqllist.Contains(throttlerecognition.Id))
                                            {
                                                throttlelist.Add(throttlerecognition);
                                            }
                                            else
                                            {
                                                continue;
                                            }

                                           
                                        }

                                    }

                                }

                            }

                        }

                        else
                        {
                            continue;
                        }



                    }

                    var ListperHalfHour = _SatictisAnalysisdataAccList.GroupBy(x => new
                    {
                        x.Chantitle,
                        x.Datadate.Value.Year,
                        x.Datadate.Value.Month,
                        x.Datadate.Value.Day,
                        x.Datadate.Value.Hour,
                        x.Datadate.Value.Minute
                    }
                   ).Select(x => new
                   {
                       Id = x.Min(a => a.Id),
                       VehicleID = vehicleid,
                       Filename = x.Min(a => a.Filename),
                       Datadate = x.Min(a => a.Datadate),
                       x.Key.Chantitle,

                       Max = x.Max(a => a.Max),
                       Min = x.Min(a => a.Min),
                       Range = x.Max(a => a.Max) - x.Min(a => a.Min),
                       Rms = x.Sum(a => a.Rms) / Math.Sqrt(x.Count()),

                   //Damage = Math.Round((double)x.Sum(a => a.Damage), 0),

                   }).OrderBy(b => b.Chantitle).ThenBy(b => b.Datadate).ToList();

                    List<SatictisAnalysisdataAcc> ListperHalfHourConvert = ListperHalfHour.Select(a => new SatictisAnalysisdataAcc
                    {
                        Id = a.Id,
                        VehicleId = a.VehicleID,
                        Filename = a.Filename,
                        Datadate = a.Datadate,
                        Chantitle = a.Chantitle,
                        Max = a.Max,
                        Min = a.Min,
                        Range = a.Range,
                        Rms = a.Rms,
                        //Damage=a.Damage

                    }).ToList();

                    //经过groupby之后的acc数据再进行判断是否数据库中已经存在，如存在则从这个list中删除这一条数据


                    //用for倒序遍历删除才行
                    for (int i = ListperHalfHourConvert.Count - 1; i >= 0; i--)
                    {
                        if (accmysqllist.Contains(ListperHalfHourConvert[i].Id))
                        {
                            ListperHalfHourConvert.Remove(ListperHalfHourConvert[i]);
                        }
                    }

                    //不能用foreach来删，因为在foreach中删除元素时，每一次删除都会导致集合的大小和元素索引值发生变化，导致在foreach中删除元素会出现异常。
                    var DListperHalfHour = _SpeeddistributionList.GroupBy(x => new
                    {

                        x.Datadate.Value.Year,
                        x.Datadate.Value.Month,
                        x.Datadate.Value.Day,
                        x.Datadate.Value.Hour,
                        x.Datadate.Value.Minute
                    }
               ).Select(x => new
               {
                   Id = x.Min(a => a.Id),
                   VehicleID = vehicleid,
                   Datadate = x.Min(a => a.Datadate),


                   _010 = x.Sum(a => a._010),
                   _1020 = x.Sum(a => a._1020),
                   _2030 = x.Sum(a => a._2030),
                   _3040 = x.Sum(a => a._3040),
                   _4050 = x.Sum(a => a._4050),
                   _5060 = x.Sum(a => a._5060),
                   _6070 = x.Sum(a => a._6070),
                   _7080 = x.Sum(a => a._7080),
                   _8090 = x.Sum(a => a._8090),
                   _90100 = x.Sum(a => a._90100),
                   _100110 = x.Sum(a => a._100110),
                   _110120 = x.Sum(a => a._110120),
                   Above120 = x.Sum(a => a.Above120)


               }).OrderBy(b => b.Datadate).ToList();

                    //var ListperHalfHourConvert= ListperHalfHour.OfType<ShAdf0979SatictisAnalysisdataAcc>().ToList();

                    List<Speeddistribution> DListperHalfHourConvert = DListperHalfHour.Select(a => new Speeddistribution
                    {
                        Id = a.Id,
                        VehicleId = a.VehicleID,
                        Datadate = a.Datadate,
                        _010 = a._010,
                        _1020 = a._1020,
                        _2030 = a._2030,
                        _3040 = a._3040,
                        _4050 = a._4050,
                        _5060 = a._5060,
                        _6070 = a._6070,
                        _7080 = a._7080,
                        _8090 = a._8090,
                        _90100 = a._90100,
                        _100110 = a._100110,
                        _110120 = a._110120,
                        Above120 = a.Above120

                    }).ToList();

                    //经过groupby之后的acc数据再进行判断是否数据库中已经存在，如存在则从这个list中删除这一条数据

                    for (int i = DListperHalfHourConvert.Count - 1; i >= 0; i--)
                    {
                        if (speedmysqllist.Contains(DListperHalfHourConvert[i].Id))
                        {
                            DListperHalfHourConvert.Remove(DListperHalfHourConvert[i]);
                        }
                    }

                    _DB.BulkInsert(gpsrecordlist);
                    _DB.BulkInsert(bumpsqllist);
                    if (Brakeisexist)
                    {
                        _DB.BulkInsert(brakesqllist);
                    }
                    if (StrgWhlAngExist)
                    {
                        _DB.BulkInsert(steeringsqllist);
                    }
                    if (AccelActuExist)
                    {
                        _DB.BulkInsert(throttlelist);
                    }

                    _DB.BulkInsert(DListperHalfHourConvert);

                    _DB.BulkInsert(ListperHalfHourConvert);
                   
                    _DB.SaveChanges();
                    can= true;

                }
                
            });
            return can;
        }
    }
}
