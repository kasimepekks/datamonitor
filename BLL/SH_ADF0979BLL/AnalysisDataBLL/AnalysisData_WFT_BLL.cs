using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.SH_ADF0979BLL
{
    public class AnalysisData_WFT_BLL : BaseBLL<SatictisAnalysisdataWft>, IAnalysisData_WFT_IBLL
    {
        private readonly IAnalysisData_WFT_IDAL _AnalysisData_WFT_DAL;
        private readonly datawatchContext _DB;
        public AnalysisData_WFT_BLL(IAnalysisData_WFT_IDAL AnalysisData_WFT_DAL, datawatchContext DB)
        {
            this._AnalysisData_WFT_DAL = AnalysisData_WFT_DAL;
            _DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._AnalysisData_WFT_DAL;
        }
        /// <summary>
        /// 执行添加distance字段并进行统计后存入数据库中
        /// </summary>
        public bool ReadandMergeWFTDataperHalfHour(string filepath)
        {
           return( _AnalysisData_WFT_DAL.ReadandMergeWFTDataperHalfHour(filepath));
        }


        public IQueryable LoadWFTDamage(DateTime sd, DateTime ed)
        {
            //这里不考虑按每一天进行合并，而是合并相同时间段的数据，不区分哪一天
            var damagelist = _DB.SatictisAnalysisdataWft.AsNoTracking().Where(a => a.Datadate >= sd && a.Datadate <= ed).GroupBy(x =>
            x.Chantitle).Select(x => new
            {
                chantitle = x.Key,
                damage = x.Sum(a => a.Damage),
                max = x.Max(a => a.Max),
                min = x.Min(a => a.Min)
            }).Where(a => a.chantitle.Contains("WFT_F")).OrderBy(b => b.chantitle);

            return damagelist;
        }

        public IQueryable LoadWFTDamageCumulation(DateTime sd, DateTime ed)
        {
            //这里不考虑按每一天进行合并，而是合并相同时间段的数据，不区分哪一天
            var damagelist = _DB.SatictisAnalysisdataWft.AsNoTracking().Where(a => a.Datadate >= sd && a.Datadate <= ed.AddDays(1)).GroupBy(x => new
            {
                x.Chantitle,
                x.Datadate.Value.Year,
                x.Datadate.Value.Month,
                x.Datadate.Value.Day
            }
            ).Select(x => new
            {
                chantitle = x.Key.Chantitle,
                datetime = x.Key.Year.ToString() + "-" + x.Key.Month.ToString() + "-" + x.Key.Day.ToString(),
                damage = Math.Round((double)x.Sum(a => a.Damage), 0),

            }).Where(a => a.chantitle.Contains("WFT_F")).OrderBy(b => b.chantitle).ThenBy(b => b.datetime);

            return damagelist;
        }
    }
}
