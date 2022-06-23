using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IBumpDistribution_IBLL : IBaseIBLL<Bumprecognition>
    {
        Task<List<double>> LoadBumpDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<int> GetBumpCount(DateTime sd, DateTime ed, string vehicleid);
    }
}
