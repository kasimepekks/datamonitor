using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.SteeringListOperation
{
   public static class SteeringReconize
    {
        public struct SteeringParemeters
        {
            public double Angle;
            public double AngularAcc;
            public double Speed;
            public double SteeringStrenth;
        }

        /// <summary>
        /// 利用Gyro_Z_list和Acc_Y_FM_list来筛选出转向加速度，前提是必须把所有小于10度的数据清零
        /// </summary>
        /// <param name="Gyro_Z_list"></param>
        /// <param name="Acc_Y_FM_list"></param>
        /// <returns></returns>
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
                    //保证转向点数大于配置里的参数才算做转向
                    if (SteeringAccTemp.Count > MyConfigforVehicleID.SteeringLastingPoints)
                    {
                        //这里选取最大值和最小值之间绝对值最大的那个数
                        SteeringAccList.Add(Math.Abs(SteeringAccTemp.Max())> Math.Abs(SteeringAccTemp.Min())? SteeringAccTemp.Max(): SteeringAccTemp.Min());
                       
                    }

                    start = 0;
                }


            }

            return SteeringAccList;
        }

        /// <summary>
        /// 直接利用转向角度来筛选出每次的转向角度，角加速度，速度和转向强度，前提是必须把所有小于10度的转角数据清零
        /// </summary>
        /// <param name="StrgWhlAngList"></param>
        /// <returns></returns>
        public static List<SteeringParemeters> GetSteering(List<double> StrgWhlAngList,List<double> AngularAccList, List<double> SpeedList, List<double> SteeringStrenthList)
        {
            List<SteeringParemeters> SteeringAngleList = new List<SteeringParemeters>();
            int start = 0, end;//识别转向开始点数和结束点数
            for (int i = 0; i < StrgWhlAngList.Count - 1; i++)
            {

                if (StrgWhlAngList[i] == 0 && StrgWhlAngList[i + 1] != 0)
                {
                    start = i + 1;
                    //BrakeStartList.Add(i + 1);
                }
                if (StrgWhlAngList[i] != 0 && StrgWhlAngList[i + 1] == 0 && start != 0)//这里一定要先找到start才开始找end，否则一开始可能先找到end就不对了
                {
                    end = i;
                    //BrakeEndList.Add(i);
                    List<double> SteeringAngelTemp = new List<double>();
                    List<double> AngularAccTemp = new List<double>();
                    List<double> SpeedTemp = new List<double>();
                    List<double> SteeringStrenthTemp = new List<double>();
                    for (int j = start; j < end; j++)
                    {
                        SteeringAngelTemp.Add(StrgWhlAngList[j]);
                        AngularAccTemp.Add(AngularAccList[j]);
                        SpeedTemp.Add(SpeedList[j]);
                        SteeringStrenthTemp.Add(SteeringStrenthList[j]);
                        
                    }
                    //保证转向点数大于配置里的参数才算做转向，防止有毛刺信号
                    if (SteeringAngelTemp.Count > MyConfigforVehicleID.SteeringLastingPoints)
                    {
                        SteeringParemeters steeringParemeters;
                        //这里选取最大值和最小值之间绝对值最大的那个数
                        steeringParemeters.Angle=Math.Abs(SteeringAngelTemp.Max()) > Math.Abs(SteeringAngelTemp.Min()) ? SteeringAngelTemp.Max() : SteeringAngelTemp.Min();
                        steeringParemeters.AngularAcc = Math.Abs(AngularAccTemp.Max()) > Math.Abs(AngularAccTemp.Min()) ? AngularAccTemp.Max() : AngularAccTemp.Min();
                        steeringParemeters.Speed = SpeedTemp.Max();
                        steeringParemeters.SteeringStrenth = Math.Abs(SteeringStrenthTemp.Max()) > Math.Abs(SteeringStrenthTemp.Min()) ? SteeringStrenthTemp.Max() : SteeringStrenthTemp.Min();
                        SteeringAngleList.Add(steeringParemeters);
                    }

                    start = 0;
                }


            }

            return SteeringAngleList;
        }
    }
}
