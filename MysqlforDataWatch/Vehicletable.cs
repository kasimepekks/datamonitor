using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class Vehicletable
    {
        public int Id { get; set; }
        public string VehicleId { get; set; }
        public string Area { get; set; }
        public string Country { get; set; }
        public byte? State { get; set; }
        public string Remarks { get; set; }
    }
}
