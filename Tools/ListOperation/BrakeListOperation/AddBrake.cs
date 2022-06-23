using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;

namespace Tools.ListOperation.BrakeListOperation
{
   public static class AddBrake
    {
        public static List<Brakerecognition> addbrakelist(List<double> l7_BrkPdlDrvrAp, List<double> l5_AccXSTLF, List<double> l1_VehSpdAvg,List<string> sqllist,string vehicleid,string name,string datetime,  bool isexist,VehicleIDPara vehicleIDPara)
        {
            
            List<Brakerecognition> brakesqllist = new List<Brakerecognition>();
            if (isexist)
            {
                if (vehicleIDPara.BrakeImport == "true")
                {

                    BrakeZero.DoZero(l7_BrkPdlDrvrAp, vehicleIDPara);
                    var brakeacclist = BrakeReconize.GetBrake(l7_BrkPdlDrvrAp, l5_AccXSTLF, l1_VehSpdAvg, vehicleIDPara);
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
                            if (!sqllist.Contains(brake.Id))
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
            }
            return brakesqllist;
        }
    }
}
