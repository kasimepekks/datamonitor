using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.ListOperation.SteeringListOperation
{
   public class SteeringDistribute
    {
        public static List<int> GetSteeringDistribution(List<double> SteeringAngleList)
        {
            List<int> SteeringAngleDistribution = new List<int>();
            int Str_60 = 0, Str_120 = 0, Str_180 = 0, Str_240 = 0, Str_300 = 0, Str_360 = 0,
                 StrN_60 = 0, StrN_120 = 0, StrN_180 = 0, StrN_240 = 0, StrN_300 = 0, StrN_360 = 0, Str_00 = 0,StrN_00 = 0;
            for (int i = 0; i < SteeringAngleList.Count; i++)
            {
                if (SteeringAngleList[i] < -360)
                {
                    StrN_360 += 1;
                }
                else if(SteeringAngleList[i] < -300)
                {
                    StrN_300 += 1;
                }
                else if (SteeringAngleList[i] < -240)
                {
                    StrN_240 += 1;
                }
                else if (SteeringAngleList[i] < -180)
                {
                    StrN_180 += 1;
                }
                else if (SteeringAngleList[i] < -120)
                {
                    StrN_120 += 1;
                }
               
                else if (SteeringAngleList[i] < -60)
                {
                    StrN_60 += 1;
                }
                else if (SteeringAngleList[i] < 0)
                {
                    StrN_00 += 1;
                }
                else if (SteeringAngleList[i] < 60)
                {
                    Str_00 += 1;
                }
                else if (SteeringAngleList[i] < 120)
                {
                    Str_60 += 1;
                }
                else if (SteeringAngleList[i] < 180)
                {
                    Str_120 += 1;
                }
                else if (SteeringAngleList[i] < 240)
                {
                    Str_180 += 1;
                }
                else if (SteeringAngleList[i] < 300)
                {
                    Str_240 += 1;
                }
                else if (SteeringAngleList[i] < 360)
                {
                    Str_300 += 1;
                }
                else 
                {
                    Str_360 += 1;
                }
            }

            SteeringAngleDistribution.Add(StrN_360);
            SteeringAngleDistribution.Add(StrN_300);
            SteeringAngleDistribution.Add(StrN_240);
            SteeringAngleDistribution.Add(StrN_180);
            SteeringAngleDistribution.Add(StrN_120);
            SteeringAngleDistribution.Add(StrN_60);
            SteeringAngleDistribution.Add(StrN_00);
            SteeringAngleDistribution.Add(Str_00);
            SteeringAngleDistribution.Add(Str_60);
            SteeringAngleDistribution.Add(Str_120);
            SteeringAngleDistribution.Add(Str_180);
            SteeringAngleDistribution.Add(Str_240);
            SteeringAngleDistribution.Add(Str_300);
            SteeringAngleDistribution.Add(Str_360);
            return SteeringAngleDistribution;
        }
    }
}
