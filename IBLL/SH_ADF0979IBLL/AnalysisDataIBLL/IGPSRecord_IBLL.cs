using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IGPSRecord_IBLL : IBaseIBLL<Gpsrecord>
    {
        Task<List<Gpsrecord>> LoadGPSRecord(DateTime sd, DateTime ed, string vehicleid, int reducetimes);

    }
}
