using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class SatictisAnalysisdataAcc
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public string Filename { get; set; }
        public DateTime? Datadate { get; set; }
        public string Chantitle { get; set; }
        public double? Max { get; set; }
        public double? Min { get; set; }
        public double? Range { get; set; }
        public double? Rms { get; set; }
    }
}
