using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.SteeringListOperation
{
   public static class SteeringReconize
    {
        public static List<double> GetSteering(List<double> Gyro_Z_list, List<double> Acc_Y_FM_list)
        {
            List<double> SteeringAccList = new List<double>();
            int start = 0, end;//识别转向开始点数和结束点数
            for (int i = 0; i < Gyro_Z_list.Count - 1; i++)
            {

                if (Gyro_Z_list[i] == 0 && Gyro_Z_list[i + 1] != 0 )
                {
                    start = i + 1;
                    //BrakeStartList.Add(i + 1);
                }
                if (Gyro_Z_list[i] != 0 && Gyro_Z_list[i + 1] == 0 && start != 0)//这里一定要先找到start才开始找end，否则一开始可能先找到end就不对了
                {
                    end = i;
                    //BrakeEndList.Add(i);
                    List<double> SteeringAccTemp = new List<double>();
                    for (int j = start; j < end; j++)
                    {
                        SteeringAccTemp.Add(Acc_Y_FM_list[j]);
                    }
                    if (SteeringAccTemp.Count > MyConfigforVehicleID.SteeringLastingPoints)
                    {
                        SteeringAccList.Add(Math.Abs(SteeringAccTemp.Max())> Math.Abs(SteeringAccTemp.Min())? SteeringAccTemp.Max(): SteeringAccTemp.Min());
                        //Console.WriteLine(start / 512 + "~" + end / 512);
                    }

                    start = 0;
                }


            }

            return SteeringAccList;
        }
    }
}
