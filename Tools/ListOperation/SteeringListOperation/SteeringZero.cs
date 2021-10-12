using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation
{
   public static class SteeringZero
    {
        /// <summary>
        /// 把参数的数据小于zero的清零，zero可在配置文件中修改
        /// </summary>
     
        public static void DoZero( List<double> Gyro_Z_list)
        {
          
            double Steeringzero = MyConfigforVehicleID.SteeringZeroStandard;
            
            for (int i = 0; i < Gyro_Z_list.Count; i++)
            {
                if (Gyro_Z_list[i] <= Steeringzero && Gyro_Z_list[i] >= -Steeringzero)
                {
                    Gyro_Z_list[i] = 0;
                }
               

            }
        }
    }
}
