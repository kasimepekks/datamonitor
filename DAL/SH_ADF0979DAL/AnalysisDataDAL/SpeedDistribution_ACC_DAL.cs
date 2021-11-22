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
using Tools.AddDistance;
using Tools.MyConfig;

namespace DAL.SH_ADF0979DAL
{
    public class SpeedDistribution_ACC_DAL : BaseDAL<Speeddistribution>, ISpeedDistribution_ACC_IDAL
    {
        public SpeedDistribution_ACC_DAL(datawatchContext DB) : base(DB)
        {

        }
        /// <summary>
        /// 把每个csv文件计算speed分布并存入数据库中，目前已经不需要此功能了，已经集成到一起了
        /// </summary>
       
        //public void ReadandMergeSpeedDistributionAcc(string filepath)
        //{

        //    FileInfo[] filelist = FileOperator.Isfileexist(filepath);//获得指定文件下的所有csv文件
        //    Encoding encoding = Encoding.Default;
        //    if(filelist.Length > 0)
        //    {
        //        List<Speeddistribution> List = new List<Speeddistribution>();
        //        foreach (var file in filelist)
        //        {
        //            if (file.Length != 0)
        //            {
        //                string[] filestring = file.Name.Split('-');//把文件名每隔“-”拿出来放在这个string数组里
        //                string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
        //                string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
        //                string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
        //                string newminute = (int.Parse(timestring[1]) / 30 * 30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
        //                string newtime = timestring[0] + ":" + newminute + ":" + "00";

        //                string datetime = date + " " + newtime;
        //                //var datestart = Convert.ToDateTime(datetime);


        //                string name = file.Name.Split('.')[0];

        //                FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //                StreamReader sr = new StreamReader(fs, encoding);

        //                string strLine = "";
        //                //记录每行记录中的各字段内容
        //                string[] aryLine = null;
        //                string[] tableHead = null;


        //                //标示列数
        //                bool IsFirst = true;
        //                //逐行读取CSV中的数据


        //                List<double> speedlist = new List<double>();

        //                List<double> singledistance = new List<double>();
        //                while ((strLine = sr.ReadLine()) != null)
        //                {

        //                    if (IsFirst == true)
        //                    {
        //                        tableHead = strLine.Split(',');
        //                        IsFirst = false;

        //                    }
        //                    else
        //                    {

        //                        aryLine = strLine.Split(',');

        //                        speedlist.Add(Convert.ToDouble(aryLine[1]));
        //                        singledistance.Add(Convert.ToDouble(aryLine[aryLine.Length - 1]));

        //                    }


        //                }
        //                sr.Close();
        //                sr.Dispose();

        //                fs.Close();
        //                fs.Dispose();
        //                if (aryLine != null && aryLine.Length > 0)
        //                {
        //                    List<double> speeddistribution = new List<double>();

        //                    speeddistribution = SpeedDistribution.CalSpeedDistribution(speedlist, singledistance);
        //                    Speeddistribution entity = new Speeddistribution();
        //                    entity.Id = name;
        //                    entity.VehicleId = MyConfigforVehicleID.VehicleID;
        //                    entity.Datadate = Convert.ToDateTime(datetime);

        //                    //entity.Datatime = TimeSpan.Parse(time);
        //                    entity._010 = speeddistribution[0];
        //                    entity._1020 = speeddistribution[1];
        //                    entity._2030 = speeddistribution[2];
        //                    entity._3040 = speeddistribution[3];
        //                    entity._4050 = speeddistribution[4];
        //                    entity._5060 = speeddistribution[5];
        //                    entity._6070 = speeddistribution[6];
        //                    entity._7080 = speeddistribution[7];
        //                    entity._8090 = speeddistribution[8];
        //                    entity._90100 = speeddistribution[9];
        //                    entity._100110 = speeddistribution[10];
        //                    entity._110120 = speeddistribution[11];
        //                    entity.Above120 = speeddistribution[12];
        //                    List.Add(entity);
                          
        //                }

                    
        //            }


        //        }

        //        var ListperHalfHour = List.GroupBy(x => new
        //        {
                    
        //            x.Datadate.Value.Year,
        //            x.Datadate.Value.Month,
        //            x.Datadate.Value.Day,
        //            x.Datadate.Value.Hour,
        //            x.Datadate.Value.Minute
        //        }
        //      ).Select(x => new
        //      {
        //          Id = x.Min(a => a.Id),
        //          VehicleID= MyConfigforVehicleID.VehicleID,
        //          Datadate = x.Min(a => a.Datadate),


        //          _010 = x.Sum(a => a._010),
        //          _1020 = x.Sum(a => a._1020),
        //          _2030 = x.Sum(a => a._2030),
        //          _3040 = x.Sum(a => a._3040),
        //          _4050 = x.Sum(a => a._4050),
        //          _5060 = x.Sum(a => a._5060),
        //          _6070 = x.Sum(a => a._6070),
        //          _7080 = x.Sum(a => a._7080),
        //          _8090 = x.Sum(a => a._8090),
        //          _90100 = x.Sum(a => a._90100),
        //          _100110 = x.Sum(a => a._100110),
        //          _110120 = x.Sum(a => a._110120),
        //          Above120 = x.Sum(a => a.Above120)
                                 

        //      }).OrderBy(b => b.Datadate).ToList();

        //        //var ListperHalfHourConvert= ListperHalfHour.OfType<ShAdf0979SatictisAnalysisdataAcc>().ToList();

        //        List<Speeddistribution> ListperHalfHourConvert = ListperHalfHour.Select(a => new Speeddistribution
        //        {
        //            Id = a.Id,
        //            VehicleId = a.VehicleID,
        //            Datadate = a.Datadate,
        //            _010 =  a._010,
        //            _1020 =  a._1020,
        //            _2030 =  a._2030,
        //            _3040 = a._3040,
        //            _4050 =  a._4050,
        //            _5060 =  a._5060,
        //            _6070 = a._6070,
        //            _7080 =  a._7080,
        //            _8090 = a._8090,
        //            _90100 = a._90100,
        //            _100110 =  a._100110,
        //            _110120 = a._110120,
        //            Above120 =  a.Above120

        //        }).ToList();
        //        _DB.BulkInsert(ListperHalfHourConvert);

        //        //this.CurrentDBSession.SaveChanges();
        //        _DB.SaveChanges();

        //    }

        //}
    }
}
