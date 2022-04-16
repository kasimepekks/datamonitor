using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AddDistance
{
   public class SpeedDistribution
    {
        public static List<double> CalSpeedDistribution(List<double> speedlist, List<double> singledistance)
        {
            List<double> speeddistribution=new  List<double>();
            
            double distance5=0, distance15=0, distance25=0, distance35=0, distance45=0, distance55=0, distance65=0, 
                distance75=0, distance85=0, distance95=0, distance105=0, distance115=0, distance125=0;
            for (int i = 0; i < speedlist.Count; i++)
            {
                if (speedlist[i]<10)
                {
                    distance5 += singledistance[i];
                }
                else if(speedlist[i] < 20)
                {
                    distance15 += singledistance[i];
                }
                else if (speedlist[i] < 30)
                {
                    distance25 += singledistance[i];
                }
                else if (speedlist[i] < 40)
                {
                    distance35 += singledistance[i];
                }
                else if (speedlist[i] <50)
                {
                    distance45 += singledistance[i];
                }
                else if (speedlist[i] < 60)
                {
                    distance55 += singledistance[i];
                }
                else if (speedlist[i] < 70)
                {
                    distance65 += singledistance[i];
                }
                else if (speedlist[i] < 80)
                {
                    distance75 += singledistance[i];
                }
                else if (speedlist[i] < 90)
                {
                    distance85 += singledistance[i];
                }
                else if (speedlist[i] < 100)
                {
                    distance95 += singledistance[i];
                }
                else if (speedlist[i] < 110)
                {
                    distance105 += singledistance[i];
                }
                else if (speedlist[i] < 120)
                {
                    distance115 += singledistance[i];
                }
                else 
                {
                    distance125 += singledistance[i];
                }
            }

            speeddistribution.Add(distance5);
            speeddistribution.Add(distance15);
            speeddistribution.Add(distance25);
            speeddistribution.Add(distance35);
            speeddistribution.Add(distance45);
            speeddistribution.Add(distance55);
            speeddistribution.Add(distance65);
            speeddistribution.Add(distance75);
            speeddistribution.Add(distance85);
            speeddistribution.Add(distance95);
            speeddistribution.Add(distance105);
            speeddistribution.Add(distance115);
            speeddistribution.Add(distance125);
            return speeddistribution;
        }
       
    }
}
