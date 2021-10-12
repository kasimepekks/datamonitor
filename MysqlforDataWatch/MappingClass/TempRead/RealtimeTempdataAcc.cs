using System;
using System.Collections.Generic;

namespace MysqlforDataWatch
{
    public  class RealtimeTempdataAcc:TempdataAccBase
    {
        
        //public double Time { get; set; }
        //public double Speed { get; set; }
        //public double Brake { get; set; }
       
        public double GyroY { get; set; }
        public double GyroX { get; set; }
        public double AccYFM { get; set; }
        public double GyroZ { get; set; }
        public double AccXFM { get; set; }
      
        public double AccZFM { get; set; }
        public double AccZWhlLF { get; set; }
        public double AccYWhlLF { get; set; }
        public double AccXWhlLF { get; set; }
        public double AccZWhlRF { get; set; }
        public double AccYWhlRF { get; set; }
        public double AccXWhlRF { get; set; }
        public double AccYWhlLR { get; set; }
        public double AccXWhlLR { get; set; }

        public double AccZWhlLR { get; set; }
        public double AccYWhlRR { get; set; }
        public double AccXWhlRR { get; set; }

        public double AccZWhlRR { get; set; }
        public double AccXRM { get; set; }
        public double AccYRM { get; set; }
        public double AccZRM { get; set; }


        //public double Lat { get; set; }
        //public double Lon { get; set; }

        //public double VehSpdAvg { get; set; }
    }
}
