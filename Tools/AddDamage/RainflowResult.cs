using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AddDamage
{
   public class RainflowResult
    {
        public double sMean;
        public double sRange;
        public double sCount;
        public RainflowResult(double mean, double range, double count)
        {
            sMean = mean;
            sRange = range;
            sCount = count;
        }
    }
}
