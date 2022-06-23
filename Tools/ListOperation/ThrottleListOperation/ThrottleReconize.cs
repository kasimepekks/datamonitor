using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.ThrottleListOperation
{
   public class ThrottleReconize
    {
        public struct ThrottleParemeters
        {
            public double throttle;
            public double accxst;
            public double Speed;
            public double lastingtime;
            public sbyte direction; 
        }

        public static List<ThrottleParemeters> GetThrottle(List<double> throttle_list, List<double> Acc_X_ST_list, List<double> Speed_list, VehicleIDPara vehicleIDPara)
        {
            List<ThrottleParemeters> throttleparemeterslist = new List<ThrottleParemeters>();
            int start = 0, end;//识别转向开始点数和结束点数
            for (int i = 0; i < throttle_list.Count - 1; i++)
            {

                if (throttle_list[i] == 0 && throttle_list[i + 1] != 0)
                {
                    start = i + 1;
                    //BrakeStartList.Add(i + 1);
                }
                if (throttle_list[i] != 0 && throttle_list[i + 1] == 0 && start != 0)//这里一定要先找到start才开始找end，否则一开始可能先找到end就不对了
                {
                    end = i;
                    //BrakeEndList.Add(i);
                    List<double> throttleTemp = new List<double>();
                    List<double> speedTemp = new List<double>();
                    List<double> accTemp = new List<double>();
                    for (int j = start; j < end; j++)
                    {
                        throttleTemp.Add(throttle_list[j]);
                        speedTemp.Add(Speed_list[j]);
                        accTemp.Add(Acc_X_ST_list[j]);
                    }
                    //保证油门点数大于配置里的参数才算做转向
                    if (throttleTemp.Count > vehicleIDPara.ThrottleLastingPoints)
                    {
                        ThrottleParemeters throttleparemeters;
                        throttleparemeters.throttle = throttleTemp.Max();
                        throttleparemeters.Speed = speedTemp.Max();
                        //这里选取最大值和最小值之间绝对值最大的那个数
                        throttleparemeters.accxst = Math.Abs(accTemp.Max()) > Math.Abs(accTemp.Min()) ? accTemp.Max() : accTemp.Min();
                        throttleparemeters.lastingtime = Math.Round(vehicleIDPara.Reductiontimesforimport * (end - start) * 1.0 / MyConfigforVehicleID.SampleRate, 2);
                        throttleparemeters.direction = (sbyte)(throttleparemeters.accxst > 0 ? 1 : 0);
                        throttleparemeterslist.Add(throttleparemeters);
                    }

                    start = 0;
                }


            }

            return throttleparemeterslist;
        }
    }
}
