using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class Gpsrecord
    {
        public long Key { get; set; }
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public string Filename { get; set; }
        public DateTime? Datadate { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public double Speed { get; set; }
    }
}
