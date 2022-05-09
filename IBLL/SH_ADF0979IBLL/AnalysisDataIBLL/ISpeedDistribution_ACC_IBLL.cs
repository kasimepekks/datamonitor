using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.SH_ADF0979IBLL
{
   public interface ISpeedDistribution_ACC_IBLL : IBaseIBLL<Speeddistribution>
    {
        //void ReadandMergeSpeedDistributionAcc(string filepath);
        Task<IQueryable> LoadSpeedDistribution(DateTime sd, DateTime ed, string vehicleid);
        Task<IQueryable> LoadSpeedDistributionperday(DateTime sd, DateTime ed, string vehicleid);
        Task<IQueryable> LoadSpeedDistributionperhour(DateTime sd, DateTime ed, string vehicleid);
        Task<List<double>> LoadTextRecord(DateTime sd, DateTime ed, string vehicleid);
    }
}
