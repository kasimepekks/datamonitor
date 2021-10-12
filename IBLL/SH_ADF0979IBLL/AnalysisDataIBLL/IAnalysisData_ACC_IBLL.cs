using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IAnalysisData_ACC_IBLL : IBaseIBLL<SatictisAnalysisdataAcc>
    {
        //bool ReadFilesForAnalysisDataAcc(string filepath);
        bool ReadandMergeACCDataperHalfHour(string filepath);
    }
}
