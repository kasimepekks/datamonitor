using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation
{
   public static class BrakeZero
    {
        /// <summary>
        /// 把参数的数据小于zero的清零，zero可在配置文件中修改
        /// </summary>
    
        public static void DoZero( List<double> brakelist, VehicleIDPara vehicleIDPara)
        {
          
            double brakezero = vehicleIDPara.BrakeZeroStandard;
            
            for (int i = 0; i < brakelist.Count; i++)
            {
                if (brakelist[i] <= brakezero)
                {
                    brakelist[i] = 0;
                }
               

            }
        }
    }
}
