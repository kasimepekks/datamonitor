using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AddDamage
{
   public static class GetDamageFromList
    {
        /// <summary>
        /// 计算给定的List<double>集合的damage并返回damage
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static double GetDamage(this List<double> lst)
        {

            RainflowCountandDamage RF = new RainflowCountandDamage();
            RF.GetPVPoints(lst);
            RF.GetCycle(RF.pv_data);
            RF.GetCombinedCycle();
            RF.GetDamage(RF.RFResult);
            return RF.damage;
        }
    }
}
