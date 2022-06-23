using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.DateTimeOperation
{
   public static class DateTimeOperation
    {
        public static int DateDiff(string dateStart, string dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart);
            DateTime end = Convert.ToDateTime(dateEnd);

            TimeSpan sp = end.Subtract(start);

            return sp.Days;
        }
    }
}
