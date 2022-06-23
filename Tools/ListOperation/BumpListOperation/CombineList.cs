using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation
{
   public static class CombineList
    {
        /// <summary>
        ///判断左前峰值出现的时刻是否很相近，如很相近则合并取最大值
        /// </summary>
        /// <param name="ValueList"></param>
        /// <param name="TimeList"></param>
        /// <param name="OuttimeList"></param>
        /// <returns></returns>
        public static List<double> CombineListMethod(List<double> ValueList,List<int> TimeList, VehicleIDPara vehicleIDPara, out List<int> OuttimeList)
        {
            
            List<double> combinedlist = new List<double>();
            List<int> outtimelist = new List<int>();
            //在做合并之前，每一个list最后都添加一个数据，数据本身没有什么意义，只是为了循环里的if到最后能够肯定执行，否则最后的峰值会丢失，也无法合并
            ValueList.Add(0);
            TimeList.Add(-vehicleIDPara.BumpTimeGap - 1);//保证最后一个时刻与前一时刻相差超过vehicleIDPara.BumpTimeGap，进而能确保进行合并操作
            int t = 1;
            if (ValueList.Count > 1)
            {
                for (int i = 0; i < TimeList.Count; i = i + t)
                {
                    for (int j = i + 1; j < TimeList.Count; j++)
                    {
                        //如果后一个数和前一个数相差不到5个点，则继续判断下一个数，直到超过BumpTimeGap才进行合并
                        if (Math.Abs(TimeList[j]- TimeList[i] )> vehicleIDPara.BumpTimeGap)
                        {
                            t = j - i;
                            List<double> newlist = new List<double>();
                            List<int> newtimelist = new List<int>();
                            for (int k = i; k < j; k++)
                            {
                                newlist.Add(ValueList[k]);
                                newtimelist.Add(TimeList[k]);
                            }
                            double tmax = newlist.Max();
                            int maxindex = newlist.IndexOf(tmax);
                            combinedlist.Add(tmax);

                            outtimelist.Add(newtimelist[maxindex]);
                            break;
                        }

                    }


                }
            }
            else if(ValueList.Count==1)
            {
                OuttimeList = TimeList;
                return ValueList;
            }
            OuttimeList = outtimelist;
            return combinedlist;
        }
    }
}
