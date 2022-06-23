using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IBrakeDistribution_IBLL : IBaseIBLL<Brakerecognition>
    {
        Task<List<double>> LoadBrakeDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<int> GetBrakeCount(DateTime sd, DateTime ed, string vehicleid);
    }
}
