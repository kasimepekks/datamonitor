using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class Throttlerecognition
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public string Filename { get; set; }
        public DateTime? Datadate { get; set; }
        public double ThrottleAcc { get; set; }
        public double Accelerograph { get; set; }
        public double Speed { get; set; }
        public double LastingTime { get; set; }
        public sbyte Reverse { get; set; }
    }
}
