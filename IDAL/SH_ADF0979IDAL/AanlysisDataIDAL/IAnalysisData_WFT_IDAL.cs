using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL.SH_ADF0979IDAL
{
    public interface IAnalysisData_WFT_IDAL : IBaseDAL<SatictisAnalysisdataWft>
    {
        bool ReadandMergeWFTDataperHalfHour(string filepath);
    }
}
