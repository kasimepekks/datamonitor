using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SH_ADF0979BLL
{
    public class AnalysisData_ACC_BLL : BaseBLL<SatictisAnalysisdataAcc>, IAnalysisData_ACC_IBLL
    {
        private readonly IAnalysisData_ACC_IDAL _AnalysisData_ACC_DAL;
        private readonly DbContext _DB;
        public AnalysisData_ACC_BLL(IAnalysisData_ACC_IDAL AnalysisData_ACC_DAL, DbContext DB)
        {
            this._AnalysisData_ACC_DAL = AnalysisData_ACC_DAL;
            base.CurrentDal = AnalysisData_ACC_DAL;//构造函数把实例传入给basebll里的currentdal


            _DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._AnalysisData_ACC_DAL;
        }
       

        public async Task<bool> ReadandMergeACCDataperHalfHour(string filepath)
        {
            return await _AnalysisData_ACC_DAL.ReadandMergeACCDataperHalfHour(filepath);
        }
    }
}
