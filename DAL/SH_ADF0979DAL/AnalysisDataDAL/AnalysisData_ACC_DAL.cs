using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Tools;
using Tools.AddDamage;
using Tools.AddDistance;
using Tools.ListOperation;
using Tools.ListOperation.BrakeListOperation;
using Tools.ListOperation.SteeringListOperation;
using Tools.MyConfig;

namespace DAL.SH_ADF0979DAL
{
  public  class AnalysisData_ACC_DAL : BaseDAL<SatictisAnalysisdataAcc>, IAnalysisData_ACC_IDAL
    {
        public AnalysisData_ACC_DAL(datawatchContext DB) : base(DB)
        {

        }

        /// <summary>
        /// 把每个csv文件计算统计值并存入数据库中
        /// </summary>
       

        public bool ReadandMergeACCDataperHalfHour(string filepath)
        {

            FileInfo[] filelist = FileOperator.Isfileexist(filepath);//获得指定文件下的所有csv文件
            Encoding encoding = Encoding.Default;
            if (filelist.Length > 0)
            {
                List<Speeddistribution> _SpeeddistributionList = new List<Speeddistribution>();
                List<SatictisAnalysisdataAcc> _SatictisAnalysisdataAccList = new List<SatictisAnalysisdataAcc>();
                List<Bumprecognition> bumpsqllist = new List<Bumprecognition>();
                List<Brakerecognition> brakesqllist = new List<Brakerecognition>();
                List<Streeringrecognition> steeringsqllist = new List<Streeringrecognition>();

                foreach (var file in filelist)
                {
                    if (file.Length != 0)
                    {
                        string[] filestring = file.Name.Split('-');//把文件名每隔“-”拿出来放在这个string数组里
                        string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
                        string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
                        string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
                        string newminute = (int.Parse(timestring[1])/30*30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
                        string newtime = timestring[0] + ":" + newminute + ":" + "00";

                        string datetime = date + " " + newtime;
                       //var datestart= Convert.ToDateTime(datetime);
                       
                     
                        string name = file.Name.Split('.')[0];

                        List<double> speedlist = new List<double>();
                        List<double> speedpeaklist = new List<double>();
                        List<double> timelist = new List<double>();
                        List<double> singledistance = new List<double>();
                        List<double> WFT_AZ_LFList= new List<double>();
                        List<double> WFT_AZ_RFList = new List<double>();
                        List<double> WFT_AZ_LRList = new List<double>();
                        List<double> WFT_AZ_LFPeakList = new List<double>();
                        List<double> WFT_AZ_RFPeakList = new List<double>();
                        List<double> WFT_AZ_LRPeakList = new List<double>();
                        List<int> WFT_AZ_LFPeakTimeList = new List<int>();
                        List<int> WFT_AZ_RFPeakTimeList = new List<int>();
                        List<int> WFT_AZ_LRPeakTimeList = new List<int>();

                        List<double> brakelist = new List<double>();
                        List<double> Acc_X_FM_list = new List<double>();

                        List<double> Gyro_Z_list = new List<double>();
                        List<double> Acc_Y_FM_list = new List<double>();
                        bool Gyro_Zisexist = false;

                        brakelist.Add(0);//每次都在开始加一个0数据
                        Gyro_Z_list.Add(0);//每次都在开始加一个0数据

                        FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                        StreamReader sr = new StreamReader(fs, encoding);
                        DataTable dt = new DataTable();
                        string strLine = "";
                        //记录每行记录中的各字段内容
                        string[] aryLine = null;
                        string[] tableHead = null;


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
                                if (tableHead.Contains("Gyro_Z"))
                                {
                                    Gyro_Zisexist = true;
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
                                aryLine = strLine.Split(',');

                                speedlist.Add(Convert.ToDouble(aryLine[1]));
                                timelist.Add(Convert.ToDouble(aryLine[0]));
                                WFT_AZ_LFList.Add(Convert.ToDouble(aryLine[9]));
                                WFT_AZ_RFList.Add(Convert.ToDouble(aryLine[12]));
                                WFT_AZ_LRList.Add(Convert.ToDouble(aryLine[17]));

                                brakelist.Add(Convert.ToDouble(aryLine[2]));

                                Acc_X_FM_list.Add(Convert.ToDouble(aryLine[7]));
                                Acc_Y_FM_list.Add(Convert.ToDouble(aryLine[5]));

                                if (Gyro_Zisexist)
                                {
                                    Gyro_Z_list.Add(Convert.ToDouble(aryLine[6]));
                                }

                                DataRow dr = dt.NewRow();
                                for (int j = 0; j < columnCount; j++)
                                {
                                    dr[j] = aryLine[j];
                                }
                                dt.Rows.Add(dr);
                            }
                        }

                        brakelist.Add(0);//每次都在最后再加一个0数据
                        Gyro_Z_list.Add(0);//每次都在最后再加一个0数据

                        sr.Close();
                        sr.Dispose();

                        fs.Close();
                        fs.Dispose();
                        if (aryLine != null && aryLine.Length > 0)
                        {

                            for (int l = 0; l < columnCount - 1; l++)
                            {

                                SatictisAnalysisdataAcc statisticentity = new SatictisAnalysisdataAcc();
                                //var idinclude = _DB.Set<ShAdf0979SatictisAnalysisdataAcc>().Select(a => a.Id);
                                statisticentity.Id = MyConfigforVehicleID.VehicleID + "-" + name + "-ACC-" + l.ToString();
                                statisticentity.VehicleId = MyConfigforVehicleID.VehicleID;
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
                            singledistance = AddDistanceList.ReturnSingleDistance(speedlist, timelist);
                            List<double> speeddistribution = new List<double>();
                           

                            speeddistribution = SpeedDistribution.CalSpeedDistribution(speedlist, singledistance);
                            Speeddistribution entity = new Speeddistribution();
                            entity.Id = MyConfigforVehicleID.VehicleID + "-" + name + "-Distribution";
                            entity.VehicleId = MyConfigforVehicleID.VehicleID;
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

                            BumpZero.DoZero(WFT_AZ_LFList, WFT_AZ_LFList, WFT_AZ_LFList);

                            PeakSelect.GetPeak(WFT_AZ_LFList, WFT_AZ_RFList, WFT_AZ_LRList, speedlist, out WFT_AZ_LFPeakList, out WFT_AZ_RFPeakList, out WFT_AZ_LRPeakList, out WFT_AZ_LFPeakTimeList, out WFT_AZ_RFPeakTimeList, out WFT_AZ_LRPeakTimeList, out speedpeaklist);
                           

                            var bumpnocombinelist= BumpReconize.GetBump(WFT_AZ_LFPeakList, WFT_AZ_RFPeakList, WFT_AZ_LRPeakList, WFT_AZ_LFPeakTimeList, WFT_AZ_RFPeakTimeList, WFT_AZ_LRPeakTimeList, speedpeaklist, speedlist, out List<int> OuttimeListnocombine);
                            if (bumpnocombinelist.Count > 0)
                            {
                                var bumpcombinedlist = CombineList.CombineListMethod(bumpnocombinelist, OuttimeListnocombine, out List<int> OuttimeList);
                               
                                //int n =1;
                                for (int i = 0; i < bumpcombinedlist.Count; i++)
                                {
                                    Bumprecognition bump = new Bumprecognition();
                                    bump.Id = MyConfigforVehicleID.VehicleID + "-" + name + "-Bump-" + i;
                                    bump.VehicleId = MyConfigforVehicleID.VehicleID;
                                    bump.Datadate = Convert.ToDateTime(datetime);
                                    bump.Filename = name;
                                    bump.BumpAcc = bumpcombinedlist[i];
                                    
                                    bumpsqllist.Add(bump);
                                }
                             
                            }

                            BrakeZero.DoZero(brakelist);
                            var brakeacclist = BrakeReconize.GetBrake(brakelist, Acc_X_FM_list, speedlist);
                            if (brakeacclist.Count > 0)
                            {
                                

                                for (int i = 0; i < brakeacclist.Count; i++)
                                {
                                    Brakerecognition brake = new Brakerecognition();
                                    brake.Id = MyConfigforVehicleID.VehicleID + "-" + name + "-Brake-" + i;
                                    brake.VehicleId = MyConfigforVehicleID.VehicleID;
                                    brake.Datadate = Convert.ToDateTime(datetime);
                                    brake.Filename = name;
                                    brake.BrakeAcc = brakeacclist[i];

                                    brakesqllist.Add(brake);
                                }
                               
                            }
                         

                            SteeringZero.DoZero(Gyro_Z_list);
                            var SteeringAccList = SteeringReconize.GetSteering(Gyro_Z_list, Acc_Y_FM_list);
                            if (SteeringAccList.Count > 0)
                            {
                                
                              
                                for (int i = 0; i < SteeringAccList.Count; i++)
                                {
                                    Streeringrecognition steering = new Streeringrecognition();
                                    steering.Id = MyConfigforVehicleID.VehicleID + "-" + name + "-Steering-" + i;
                                    steering.VehicleId = MyConfigforVehicleID.VehicleID;
                                    steering.Datadate = Convert.ToDateTime(datetime);
                                    steering.Filename = name;
                                    steering.SteeringAcc = SteeringAccList[i];
                                    steering.SteeringDirection = (sbyte)(i > 0 ? 1 : -1);
                                   
                                    steeringsqllist.Add(steering);
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
                   VehicleID = MyConfigforVehicleID.VehicleID,
                   Filename = x.Min(a => a.Filename),
                   Datadate = x.Min(a => a.Datadate),
                   x.Key.Chantitle,
                  
                   Max = x.Max(a => a.Max),
                   Min = x.Min(a => a.Min),
                   Range = x.Max(a => a.Max) - x.Min(a => a.Min),
                   Rms=x.Sum(a=>a.Rms)/ Math.Sqrt(x.Count()),
   
                   //Damage = Math.Round((double)x.Sum(a => a.Damage), 0),

               }).OrderBy(b => b.Chantitle).ThenBy(b => b.Datadate).ToList();

                //var ListperHalfHourConvert= ListperHalfHour.OfType<ShAdf0979SatictisAnalysisdataAcc>().ToList();

                List<SatictisAnalysisdataAcc> ListperHalfHourConvert = ListperHalfHour.Select(a => new SatictisAnalysisdataAcc
                {
                    Id=a.Id,
                    VehicleId = a.VehicleID,
                    Filename =a.Filename,
                    Datadate=a.Datadate,
                    Chantitle=a.Chantitle,
                    Max=a.Max,
                    Min=a.Min,
                    Range=a.Range,
                    Rms=a.Rms,
                    //Damage=a.Damage

                }).ToList();

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
               VehicleID = MyConfigforVehicleID.VehicleID,
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



                _DB.BulkInsert(bumpsqllist);
                _DB.BulkInsert(brakesqllist);
                _DB.BulkInsert(steeringsqllist);

                _DB.BulkInsert(DListperHalfHourConvert);
                
                _DB.BulkInsert(ListperHalfHourConvert);
                //this.CurrentDBSession.SaveChanges();
                _DB.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }

        }
    }
}
