using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.ListOperation.StatisticAccListOperation
{
    public static class WFTCombined
    {
        public static List<SatictisAnalysisdataAcc> statisticcombine(List<SatictisAnalysisdataAcc> orlist,List<string> sqllist,string vehicleid)
        {
            if (orlist != null)
            {
                var ListperHalfHour = orlist.GroupBy(x => new
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
                    if (sqllist.Contains(ListperHalfHourConvert[i].Id))
                    {
                        ListperHalfHourConvert.Remove(ListperHalfHourConvert[i]);
                    }
                }
                return ListperHalfHourConvert;
            }
            else
            {
                return null;
            }

        }
    }
}
