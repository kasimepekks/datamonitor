using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IDAL.SH_ADF0979IDAL
{
    public interface IAnalysisData_WFT_IDAL : IBaseDAL<SatictisAnalysisdataWft>
    {
        Task<bool> ReadandMergeWFTDataperHalfHour(string filepath, string vehicleid);
    }
}
