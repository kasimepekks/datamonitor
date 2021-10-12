using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class Speeddistribution
    {
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public DateTime? Datadate { get; set; }
        public double? _010 { get; set; }
        public double? _1020 { get; set; }
        public double? _2030 { get; set; }
        public double? _3040 { get; set; }
        public double? _4050 { get; set; }
        public double? _5060 { get; set; }
        public double? _6070 { get; set; }
        public double? _7080 { get; set; }
        public double? _8090 { get; set; }
        public double? _90100 { get; set; }
        public double? _100110 { get; set; }
        public double? _110120 { get; set; }
        public double? Above120 { get; set; }
    }
}
