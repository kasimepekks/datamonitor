using System;
using System.Collections.Generic;

namespace MysqlforDataWatch
{
    public  class TempdataAccBase
    {
        
        public double Time { get; set; }
        public double Speed { get; set; }
        public double Brake { get; set; }
      
        public double Lat { get; set; }
        public double Lon { get; set; }

        public double GPSSpeed { get; set; }
    }
}
