using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.SteeringListOperation
{
   public static class AddSteering
    {
        public static List<Streeringrecognition> addsteeringlist(List<double> l9_StrgWhlAng, List<double> l10_StrgWhlAngGr,  List<double> l1_VehSpdAvg, List<double> l6_AccYSTLF, List<string> sqllist, string vehicleid, string name, string datetime, bool isexist,VehicleIDPara vehicleIDPara)
        {

            List<Streeringrecognition> steeringsqllist = new List<Streeringrecognition>();
            if (isexist)
            {
                if (vehicleIDPara.SteeringImport == "true")
                {

                    SteeringZero.DoZero(l9_StrgWhlAng, vehicleIDPara);

                    var SteeringAccList = SteeringReconize.GetSteering(l9_StrgWhlAng, l10_StrgWhlAngGr, l1_VehSpdAvg, l6_AccYSTLF, vehicleIDPara);
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
                            if (!sqllist.Contains(steering.Id))
                            {
                                steeringsqllist.Add(steering);
                            }
                            else
                            {
                                continue;
                            }

                        }

                    }

                }
            }
            


            return steeringsqllist;
        }
    }
}
