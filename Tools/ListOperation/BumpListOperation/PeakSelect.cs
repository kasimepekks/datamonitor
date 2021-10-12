using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.ListOperation
{
  public static  class PeakSelect
    {
        /// <summary>
        /// 取在速度小于45的4个轮心的波峰值并返回
        /// </summary>
        /// <param name="WFT_AZ_LFList"></param>
        /// <param name="WFT_AZ_RFList"></param>
        /// <param name="WFT_AZ_LRList"></param>
        /// <param name="speedlist"></param>
        /// <param name="WFT_AZ_LFPeakList"></param>
        /// <param name="WFT_AZ_RFPeakList"></param>
        /// <param name="WFT_AZ_LRPeakList"></param>
        /// <param name="WFT_AZ_LFPeakTimeList"></param>
        /// <param name="WFT_AZ_RFPeakTimeList"></param>
        /// <param name="WFT_AZ_LRPeakTimeList"></param>
        /// <param name="speedpeaklist"></param>
        public static void GetPeak(List<double> WFT_AZ_LFList, List<double> WFT_AZ_RFList, List<double> WFT_AZ_LRList, List<double> speedlist,out List<double> WFT_AZ_LFPeakList, out List<double> WFT_AZ_RFPeakList, out List<double> WFT_AZ_LRPeakList, out List<int> WFT_AZ_LFPeakTimeList, out List<int> WFT_AZ_RFPeakTimeList, out List<int> WFT_AZ_LRPeakTimeList, out List<double> speedpeaklist)
        {
            List<double> _WFT_AZ_LFPeakList = new List<double>();
            List<double> _WFT_AZ_RFPeakList = new List<double>();
            List<double> _WFT_AZ_LRPeakList = new List<double>();
            List<int> _WFT_AZ_LFPeakTimeList = new List<int>();
            List<int> _WFT_AZ_RFPeakTimeList = new List<int>();
            List<int> _WFT_AZ_LRPeakTimeList = new List<int>();
            List<double> _speedpeaklist = new List<double>();
            for (int i = 0; i < speedlist.Count - 2; i++)
            {
                if (speedlist[i + 1] < 50)
                {
                    if (WFT_AZ_LFList[i] < WFT_AZ_LFList[i + 1] && WFT_AZ_LFList[i + 1] > WFT_AZ_LFList[i + 2])
                    {
                        _WFT_AZ_LFPeakList.Add(WFT_AZ_LFList[i + 1]);
                        _WFT_AZ_LFPeakTimeList.Add(i + 1);
                        _speedpeaklist.Add(speedlist[i + 1]);
                    }
                    if (WFT_AZ_RFList[i] < WFT_AZ_RFList[i + 1] && WFT_AZ_RFList[i + 1] > WFT_AZ_RFList[i + 2])
                    {
                        _WFT_AZ_RFPeakList.Add(WFT_AZ_RFList[i + 1]);
                        _WFT_AZ_RFPeakTimeList.Add(i + 1);
                    }
                    if (WFT_AZ_LRList[i] < WFT_AZ_LRList[i + 1] && WFT_AZ_LRList[i + 1] > WFT_AZ_LRList[i + 2])
                    {
                        _WFT_AZ_LRPeakList.Add(WFT_AZ_LRList[i + 1]);
                        _WFT_AZ_LRPeakTimeList.Add(i + 1);
                    }

                }

            }

            WFT_AZ_LFPeakList = _WFT_AZ_LFPeakList;
            WFT_AZ_RFPeakList = _WFT_AZ_RFPeakList;
            WFT_AZ_LRPeakList = _WFT_AZ_LRPeakList;
            WFT_AZ_LFPeakTimeList = _WFT_AZ_LFPeakTimeList;
            WFT_AZ_RFPeakTimeList = _WFT_AZ_RFPeakTimeList;
            WFT_AZ_LRPeakTimeList = _WFT_AZ_LRPeakTimeList;
            speedpeaklist = _speedpeaklist;

            //for (int i = 0; i < _WFT_AZ_LFPeakList.Count; i++)
            //{
            //    Console.WriteLine("LFP," + _WFT_AZ_LFPeakList[i]);
            //    Console.WriteLine("LFT," + _WFT_AZ_LFPeakTimeList[i]);
            //}
            //for (int i = 0; i < _WFT_AZ_RFPeakList.Count; i++)
            //{
            //    Console.WriteLine("RFP," + _WFT_AZ_RFPeakList[i]);
            //    Console.WriteLine("RFT," + _WFT_AZ_RFPeakTimeList[i]);
            //}
            //for (int i = 0; i < _WFT_AZ_LFPeakList.Count; i++)
            //{
            //    Console.WriteLine("LRP," + _WFT_AZ_LRPeakList[i]);
            //    Console.WriteLine("LRT," + _WFT_AZ_LRPeakTimeList[i]);
            //}
        }
    }
}
