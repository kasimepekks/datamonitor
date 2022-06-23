using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.ThrottleListOperation
{
    public static class ThrottleZero
    {
        public static void DoZero(List<double> throttlelist, VehicleIDPara vehicleIDPara)
        {

            double throttlezero = vehicleIDPara.ThrottleZeroStandard;

            for (int i = 0; i < throttlelist.Count; i++)
            {
                if (throttlelist[i] <= throttlezero)
                {
                    throttlelist[i] = 0;
                }


            }
        }

    }
}
