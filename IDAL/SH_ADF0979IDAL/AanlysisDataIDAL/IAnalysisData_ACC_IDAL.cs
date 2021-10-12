using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL.SH_ADF0979IDAL
{
   public interface IAnalysisData_ACC_IDAL : IBaseDAL<SatictisAnalysisdataAcc>
    {
        //bool ReadFilesForAnalysisDataAcc(string filepath);
        bool ReadandMergeACCDataperHalfHour(string filepath);
    }
}
