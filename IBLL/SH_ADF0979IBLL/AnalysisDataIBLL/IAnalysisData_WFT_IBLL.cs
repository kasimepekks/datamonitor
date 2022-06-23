using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IAnalysisData_WFT_IBLL : IBaseIBLL<SatictisAnalysisdataWft>
    {
        Task<bool> ReadandMergeWFTDataperHalfHour(string filepath, string vehicleid, VehicleIDPara vehicleIDPara);
        Task<IQueryable> LoadWFTDamage(DateTime sd, DateTime ed,string vehicleid);
        Task<IQueryable> LoadWFTDamageCumulation(DateTime sd, DateTime ed, string vehicleid);
    }
}
