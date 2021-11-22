using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IAnalysisData_WFT_IBLL : IBaseIBLL<SatictisAnalysisdataWft>
    {
        bool  ReadandMergeWFTDataperHalfHour(string filepath);
        IQueryable LoadWFTDamage(DateTime sd, DateTime ed,string vehicleid);
        IQueryable LoadWFTDamageCumulation(DateTime sd, DateTime ed, string vehicleid);
    }
}
