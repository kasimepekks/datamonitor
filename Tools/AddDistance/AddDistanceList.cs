using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AddDistance
{
   public class AddDistanceList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="time"></param>
        /// <param name="distance">输出单个里程</param>
        /// <returns></returns>
        public static List<double> AddDistanceToCSV(List<double>speed,List<double> time,out List<double> distance)
        {
            //累积里程
            List<double> AccumulatedDistance = new List<double>();
            List<double> Distance = new List<double>();
            AccumulatedDistance.Add(0);
            Distance.Add(0);
            double accumulatedistance=0,singledistance;
            for (int i = 0; i < speed.Count-1; i++)
            {

                accumulatedistance += (speed[i] + speed[i + 1]) * (time[i + 1] - time[i]) / 2 / 3.6;
                singledistance = (speed[i] + speed[i + 1]) * (time[i + 1] - time[i]) / 2 / 3.6;
                AccumulatedDistance.Add(accumulatedistance);
                Distance.Add(singledistance);
            }
            distance = Distance;
            return AccumulatedDistance;
        }

        /// <summary>
        /// 只返回单个里程，不返回累积里程
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<double> ReturnSingleDistance(List<double> speed, List<double> time)
        {
            
           
            List<double> Distance = new List<double>();
           
            Distance.Add(0);
            double singledistance;
            for (int i = 0; i < speed.Count - 1; i++)
            {

                singledistance = (speed[i] + speed[i + 1]) * (time[i + 1] - time[i]) / 2 / 3.6;
               
                Distance.Add(singledistance);
            }
           
            return Distance;
        }
    }
}
