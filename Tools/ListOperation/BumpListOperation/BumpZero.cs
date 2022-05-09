using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation
{
   public static class BumpZero
    {
        /// <summary>
        /// 把参数的数据小于zero的清零，zero可在配置文件中修改
        /// </summary>
        /// <param name="WFT_AZ_LFList"></param>
        /// <param name="WFT_AZ_RFList"></param>
        /// <param name="WFT_AZ_LRList"></param>
        public static void DoZero(List<double> WFT_AZ_LFList, List<double> WFT_AZ_RFList, List<double> WFT_AZ_LRList, VehicleIDPara vehicleIDPara)
        {
            double bumpzero = vehicleIDPara.BmupZeroStandard;
            
            for (int i = 0; i < WFT_AZ_LFList.Count; i++)
            {
                if (WFT_AZ_LFList[i] <= bumpzero)
                {
                    WFT_AZ_LFList[i] = 0;
                }
                if (WFT_AZ_RFList[i] <= bumpzero)
                {
                    WFT_AZ_RFList[i] = 0;
                }
                if (WFT_AZ_LRList[i] <= bumpzero)
                {
                    WFT_AZ_LRList[i] = 0;
                }

            }
          
        }
    }
}
