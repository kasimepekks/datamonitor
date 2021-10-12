﻿using System;
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
        public static List<double> CombineListMethod(List<double> ValueList,List<int> TimeList,out List<int> OuttimeList)
        {
            //for (int i = 0; i < ValueList.Count; i++)
            //{
            //    Console.WriteLine("LFZbump," + ValueList[i]);
            //    Console.WriteLine("LFZbumpTIME," + TimeList[i]);
               
            //}
            List<double> combinedlist = new List<double>();
            List<int> outtimelist = new List<int>();
            //在做合并之前，每一个list最后都添加一个数据，数据本身没有什么意义，只是为了循环里的if到最后能够肯定执行，否则最后的峰值会丢失，也无法合并
            ValueList.Add(0);
            TimeList.Add(-MyConfigforVehicleID.BumpTimeGap - 1);
            int t = 1;
            if (ValueList.Count > 1)
            {
                for (int i = 0; i < TimeList.Count; i = i + t)
                {
                    for (int j = i + 1; j < TimeList.Count; j++)
                    {
                        if (Math.Abs(TimeList[j]- TimeList[i] )> MyConfigforVehicleID.BumpTimeGap)
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
