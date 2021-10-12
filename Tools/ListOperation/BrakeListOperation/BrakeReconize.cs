using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation.BrakeListOperation
{
  public static  class BrakeReconize
    {
        /// <summary>
        /// 识别刹车强度
        /// </summary>
        /// <param name="brakelist"></param>
        /// <param name="Acc_X_FM_list"></param>
        /// <param name="speedlist"></param>
        /// <returns></returns>
        public static List<double> GetBrake(List<double> brakelist, List<double> Acc_X_FM_list, List<double> speedlist)
        {
            //List<int> BrakeStartList = new List<int>();
            //List<int> BrakeEndList = new List<int>();
            List<double> BrakeAccList = new List<double>();
            int start=0, end;//识别刹车开始点数和结束点数
            for (int i = 0; i < brakelist.Count-1; i++)
            {
               
                 if (brakelist[i] == 0 && brakelist[i + 1] > 0 && speedlist[i+1] > 10)
                 {
                     start = i + 1;
                     //BrakeStartList.Add(i + 1);
                 }
                 if (brakelist[i] > 0 && brakelist[i + 1] == 0&&start!=0)//这里一定要先找到start才开始找end，否则一开始可能先找到end就不对了
                 {
                     end = i;
                     //BrakeEndList.Add(i);
                    List<double> BrakeAccTemp = new List<double>();
                    for (int j = start; j < end; j++)
                    {
                        BrakeAccTemp.Add(Acc_X_FM_list[j]);
                    }
                    if (BrakeAccTemp.Count > MyConfigforVehicleID.BrakeLastingPoints)
                    {
                        BrakeAccList.Add(BrakeAccTemp.Max());
                        //Console.WriteLine(start / 512 + "~" + end / 512);
                    }
                   
                    start = 0;
                 }
               
             
            }

            return BrakeAccList;

        }
    }
}
