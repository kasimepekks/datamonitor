using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Tools.AddDamage;
using Tools.MyConfig;

namespace Tools.ListOperation.WFTListOperation
{
   public static class AddWFT
    {
        public static List<SatictisAnalysisdataWft> addwft(int columnCount, string[] tableHead, DataTable dt, string vehicleid, string name, string datetime, VehicleIDPara vehicleIDPara)
        {
            List<SatictisAnalysisdataWft> wftsqllist = new List<SatictisAnalysisdataWft>();
            if (vehicleIDPara.WFTImport == "true")
            {
                for (int l = 0; l < columnCount - 1; l++)
                {

                    SatictisAnalysisdataWft entity = new SatictisAnalysisdataWft();

                    entity.Id = vehicleid + "-" + name + "-WFT-" + l.ToString();
                    entity.VehicleId = vehicleid;
                    entity.Filename = name;
                    entity.Datadate = Convert.ToDateTime(datetime);
                    entity.Chantitle = tableHead[l + 1];

                    //注意这里由于计算的列的格式是string类型，用max或min计算会出问题，所以必须先转换再求maxmin


                    entity.Max = dt.AsEnumerable().Max(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));
                    entity.Min = dt.AsEnumerable().Min(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));


                    List<double> damage = dt.AsEnumerable().Select(a => Convert.ToDouble(a.Field<string>(tableHead[l + 1]))).ToList();
                    entity.Damage = GetDamageFromList.GetDamage(damage);

                    List<string> lst = (from d in dt.AsEnumerable() select d.Field<string>(tableHead[l + 1])).ToList();
                    double t = 0;
                    int n = lst.Count;
                    foreach (var data in lst)
                    {

                        t += Convert.ToDouble(data) * Convert.ToDouble(data);
                    }


                    entity.Rms = System.Math.Sqrt(t / n);

                    entity.Range = entity.Max - entity.Min;



                    wftsqllist.Add(entity);


                }
            }
            return wftsqllist;
        }
    }
}
