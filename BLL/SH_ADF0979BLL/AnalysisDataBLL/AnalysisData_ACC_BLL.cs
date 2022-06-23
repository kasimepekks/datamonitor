using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.MyConfig;

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
       

        public async Task<bool> ReadandMergeACCDataperHalfHour(string filepath,string vehicleid, VehicleIDPara vehicleIDPara)
        {
            return await _AnalysisData_ACC_DAL.ReadandMergeACCDataperHalfHour(filepath, vehicleid, vehicleIDPara);
        }
        /// <summary>
        /// 根据日期范围加载加速度和位移统计数据到前端
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="ed"></param>
        /// <param name="vehicleid"></param>
        /// <returns></returns>
        public async Task<IQueryable> LoadACCandDisData(DateTime sd, DateTime ed, string vehicleid)
        {

            var accanddislist = await Task.Run(() => _AnalysisData_ACC_DAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).AsNoTracking().GroupBy(x =>
                 x.Chantitle).Select(x => new
                 {
                     chantitle = x.Key,
                     
                     max = x.Max(a => a.Max),
                     min = x.Min(a => a.Min)
                 }).Where(a => a.chantitle.Contains("Acc")|| a.chantitle.Contains("Dis")).OrderBy(b => b.chantitle));
            return accanddislist;
        }

    }
}
