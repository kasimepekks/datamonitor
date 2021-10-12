using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class Bumprecognition
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public string Filename { get; set; }
        public DateTime? Datadate { get; set; }
        public double BumpAcc { get; set; }
    }
}
