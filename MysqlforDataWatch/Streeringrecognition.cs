using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class Streeringrecognition
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public string Filename { get; set; }
        public DateTime? Datadate { get; set; }
        public double StrgWhlAng { get; set; }
        public double AngularAcc { get; set; }
        public double Speed { get; set; }
        public double SteeringAcc { get; set; }
        public sbyte? SteeringDirection { get; set; }
    }
}
