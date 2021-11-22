using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL.SH_ADF0979IBLL
{
   public interface ISpeedDistribution_ACC_IBLL : IBaseIBLL<Speeddistribution>
    {
        //void ReadandMergeSpeedDistributionAcc(string filepath);
        IQueryable LoadSpeedDistribution(DateTime sd, DateTime ed, string vehicleid);
        IQueryable LoadSpeedDistributionperday(DateTime sd, DateTime ed, string vehicleid);
        IQueryable LoadSpeedDistributionperhour(DateTime sd, DateTime ed, string vehicleid);
    }
}
