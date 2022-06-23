using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IThrottleDistribution_IBLL : IBaseIBLL<Throttlerecognition>
    {
        Task<IQueryable> LoadThrottleDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<int> GetThrottleCount(DateTime sd, DateTime ed, string vehicleid);
    }
}
