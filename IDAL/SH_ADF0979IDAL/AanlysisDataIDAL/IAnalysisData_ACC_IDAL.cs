using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;

namespace IDAL.SH_ADF0979IDAL
{
   public interface IAnalysisData_ACC_IDAL : IBaseDAL<SatictisAnalysisdataAcc>
    {
        //bool ReadFilesForAnalysisDataAcc(string filepath);
        Task<bool> ReadandMergeACCDataperHalfHour(string filepath, string vehicleid, VehicleIDPara vehicleIDPara);
    }
}
