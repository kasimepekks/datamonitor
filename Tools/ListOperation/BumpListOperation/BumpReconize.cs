using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.ListOperation
{
   public static class BumpReconize
    {
        /// <summary>
        /// 判断识别是否是bump，根据左右前轮峰值出现的时刻和大小，以及左后轮与前轮峰值出现的时刻来判断
        /// </summary>
        /// <param name="WFT_AZ_LFPeakList"></param>
        /// <param name="WFT_AZ_RFPeakList"></param>
        /// <param name="WFT_AZ_LRPeakList"></param>
        /// <param name="WFT_AZ_LFPeakTimeList"></param>
        /// <param name="WFT_AZ_RFPeakTimeList"></param>
        /// <param name="WFT_AZ_LRPeakTimeList"></param>
        /// <param name="speedpeaklist"></param>
        /// <param name="speedlist"></param>
        /// <param name="OuttimeList"></param>
        /// <returns></returns>
        public static List<double> GetBump(List<double> WFT_AZ_LFPeakList, List<double> WFT_AZ_RFPeakList, List<double> WFT_AZ_LRPeakList, List<int> WFT_AZ_LFPeakTimeList, List<int> WFT_AZ_RFPeakTimeList, List<int> WFT_AZ_LRPeakTimeList, List<double> speedpeaklist, List<double> speedlist, VehicleIDPara vehicleIDPara, out List<int> OuttimeList)
        {
            List<double> bumplist = new List<double>();
            List<int> bumptimelist = new List<int>();
            int s1=0,s2 = 0;//RF和LR初始循环从哪里开始，如果还没有找到，则还是从0开始，如果已经找到了则从找到的后一位开始循环
            if (WFT_AZ_LFPeakList.Count > 0 && WFT_AZ_RFPeakList.Count > 0 && WFT_AZ_LRPeakList.Count > 0)
            {

                for (int i = 0; i < WFT_AZ_LFPeakTimeList.Count; i++)
                {
                    for (int j = s1; j < WFT_AZ_RFPeakTimeList.Count; j++)
                    {
                        //如果LF的波峰值的时刻和RF的波峰值的时刻相近,这里的5需要动态更改
                        if (Math.Abs(WFT_AZ_LFPeakTimeList[i] - WFT_AZ_RFPeakTimeList[j]) < vehicleIDPara.AccTimeGap)
                        {
                            s1++;
                            //如果LF的波峰值和RF的波峰值相近，这里的0.5需要动态更改
                            if (Math.Abs(WFT_AZ_LFPeakList[i] - WFT_AZ_RFPeakList[j]) < vehicleIDPara.AccValueGap && speedpeaklist[i]>5)
                            {

                                //通过轴距，LF出现波峰的时刻的瞬时速度，采样率来计算左前和左后之间的点数差距，后面还要用平均速度来修正
                                int ogappointslower = Convert.ToInt32(vehicleIDPara.Wheelbaselower * 3.6 * 512 / speedpeaklist[i]);
                                int ogappointsupper = Convert.ToInt32(vehicleIDPara.Wheelbaseupper * 3.6 * 512 / speedpeaklist[i]);
                                double t1 = 0,t2=0;
                                if(WFT_AZ_LFPeakTimeList[i]+ ogappointsupper <= 5120)//这里必须要判断LF出现peak以后LR是否出现在下一个文件里，也就是说LF的peak出现的太晚导致LR的peak没有出现
                                {
                                    for (int l = WFT_AZ_LFPeakTimeList[i]; l < WFT_AZ_LFPeakTimeList[i] + ogappointslower; l++)
                                    {
                                        t1 += speedlist[l];
                                    }
                                    for (int l = WFT_AZ_LFPeakTimeList[i]; l < WFT_AZ_LFPeakTimeList[i] + ogappointsupper; l++)
                                    {
                                        t2 += speedlist[l];
                                    }
                                    int realgappointslower = Convert.ToInt32(vehicleIDPara.Wheelbaselower * 3.6 * 512 / (t1 / ogappointslower));
                                    int realgappointsupper = Convert.ToInt32(vehicleIDPara.Wheelbaseupper * 3.6 * 512 / (t2 / ogappointsupper));

                                    for (int k = s2; k < WFT_AZ_LRPeakTimeList.Count; k++)
                                    {

                                        //如果LF的波峰值的时刻和LR的波峰值的时刻相差一个特定值（轴距范围内）,这里的特定值需要动态更改
                                        if ((WFT_AZ_LRPeakTimeList[k] - WFT_AZ_LFPeakTimeList[i]) < realgappointsupper && (WFT_AZ_LRPeakTimeList[k] - WFT_AZ_LFPeakTimeList[i]) > realgappointslower)
                                        {
                                            s2++;
                                            bumplist.Add(WFT_AZ_LFPeakList[i]);
                                            bumptimelist.Add(WFT_AZ_LFPeakTimeList[i]);
                                            break;
                                        }
                                    }

                                }



                            }


                            break;
                        }
                        
                    }
                }
               
            }

            OuttimeList = bumptimelist;
            return bumplist;

        }
    }
}
