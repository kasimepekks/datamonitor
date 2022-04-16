using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IBLL.SH_ADF0979IBLL
{
   public interface IAnalysisData_ACC_IBLL : IBaseIBLL<SatictisAnalysisdataAcc>
    {
        //bool ReadFilesForAnalysisDataAcc(string filepath);
        Task<bool> ReadandMergeACCDataperHalfHour(string filepath);
    }
}
