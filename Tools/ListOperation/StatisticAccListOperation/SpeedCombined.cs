using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.ListOperation.StatisticAccListOperation
{
    public static class SpeedCombined
    {
        public static List<Speeddistribution> speedcombine(List<Speeddistribution> orlist,List<string> sqllist,string vehicleid)
        {
            if (orlist != null)
            {
                var DListperHalfHour = orlist.GroupBy(x => new
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
                    if (sqllist.Contains(DListperHalfHourConvert[i].Id))
                    {
                        DListperHalfHourConvert.Remove(DListperHalfHourConvert[i]);
                    }
                }
                return DListperHalfHourConvert;
            }
            else
            {
                return null;
            }

        }
    }
}
