using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.StatisticAccListOperation
{
    public static class AddStatisticacc
    {
        public static List<SatictisAnalysisdataAcc> addstatisticlist(int columncount, string[] tablehead, DataTable dt, string vehicleid, string name, string datetime, VehicleIDPara vehicleIDPara)
        {

            List<SatictisAnalysisdataAcc> statisitcsqllist = new List<SatictisAnalysisdataAcc>();
            if (vehicleIDPara.StatisticImport == "true")
            {
                for (int l = 0; l < columncount - 1; l++)
                {
                   
                    SatictisAnalysisdataAcc statisticentity = new SatictisAnalysisdataAcc();

                    statisticentity.Id = vehicleid + "-" + name + "-ACC-" + l.ToString();
                    statisticentity.VehicleId = vehicleid;
                    statisticentity.Filename = name;
                    statisticentity.Datadate = Convert.ToDateTime(datetime);
                    statisticentity.Chantitle = tablehead[l + 1];

                    //注意这里由于计算的列的格式是string类型，用max或min计算会出问题，所以必须先转换再求maxmin
                    

                    statisticentity.Max = dt.AsEnumerable().Max(s => Convert.ToDouble(s.Field<string>(tablehead[l + 1])));
                    statisticentity.Min = dt.AsEnumerable().Min(s => Convert.ToDouble(s.Field<string>(tablehead[l + 1])));

                    List<string> lst = (from d in dt.AsEnumerable() select d.Field<string>(tablehead[l + 1])).ToList();
                    double t = 0;
                    int n = lst.Count;
                    foreach (var data in lst)
                    {

                        t += Convert.ToDouble(data) * Convert.ToDouble(data);
                    }


                    statisticentity.Rms = System.Math.Sqrt(t / n);
                   
                    statisticentity.Range = statisticentity.Max - statisticentity.Min;

                    statisitcsqllist.Add(statisticentity);
                }
               
            }
            return statisitcsqllist;
        }
    }
}
