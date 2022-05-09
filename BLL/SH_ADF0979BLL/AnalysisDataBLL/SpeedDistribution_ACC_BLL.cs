using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SH_ADF0979BLL
{
    public class SpeedDistribution_ACC_BLL : BaseBLL<Speeddistribution>, ISpeedDistribution_ACC_IBLL
    {
        private readonly ISpeedDistribution_ACC_IDAL _ISpeedDistribution_ACC_DAL;
        //private readonly datawatchContext _DB;
        public SpeedDistribution_ACC_BLL(ISpeedDistribution_ACC_IDAL ISpeedDistribution_ACC_DAL)
        {
            this._ISpeedDistribution_ACC_DAL = ISpeedDistribution_ACC_DAL;
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._ISpeedDistribution_ACC_DAL;
        }
        //public void  ReadandMergeSpeedDistributionAcc(string filepath)
        //{
        //    _ISpeedDistribution_ACC_DAL.ReadandMergeSpeedDistributionAcc(filepath);
        //}
        /// <summary>
        /// 统计速度的分布并发给前端
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public async Task<IQueryable> LoadSpeedDistribution(DateTime sd, DateTime ed, string vehicleid)
        {
            
            var speeddistributionlist = await Task.Run(() => _ISpeedDistribution_ACC_DAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).AsNoTracking().GroupBy(x => new { }).Select(q => new
            {
                Time = sd + "-" + ed,
                sum0_10 = q.Sum(x => x._010),
                sum10_20 = q.Sum(x => x._1020),
                sum20_30 = q.Sum(x => x._2030),
                sum30_40 = q.Sum(x => x._3040),
                sum40_50 = q.Sum(x => x._4050),
                sum50_60 = q.Sum(x => x._5060),
                sum60_70 = q.Sum(x => x._6070),
                sum70_80 = q.Sum(x => x._7080),
                sum80_90 = q.Sum(x => x._8090),
                sum90_100 = q.Sum(x => x._90100),
                sum100_110 = q.Sum(x => x._100110),
                sum110_120 = q.Sum(x => x._110120),
                sumabove120 = q.Sum(x => x.Above120),


            }));

            return speeddistributionlist;


        }

        /// <summary>
        /// 按每天来统计里程
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public async Task<IQueryable> LoadSpeedDistributionperday(DateTime sd, DateTime ed, string vehicleid)
        {
            var speeddistributionlist = await Task.Run(() => _ISpeedDistribution_ACC_DAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).AsNoTracking().GroupBy(x => new
            {
                x.Datadate.Value.Year,
                x.Datadate.Value.Month,
                x.Datadate.Value.Day
            }).Select(x => new
            {
                day = x.Key,
                Distance = x.Sum(a => a._010) + x.Sum(a => a._100110) + x.Sum(a => a._1020) + x.Sum(a => a._110120) + x.Sum(a => a._2030) + x.Sum(a => a._3040)
                + x.Sum(a => a._4050) + x.Sum(a => a._5060) + x.Sum(a => a._6070) + x.Sum(a => a._7080) + x.Sum(a => a._8090) + x.Sum(a => a._90100)
            }));


            return speeddistributionlist;
        }
        /// <summary>
        /// 按时间段来统计里程，不区分哪一天
        /// </summary>
        /// <param name="sd"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public async Task<IQueryable> LoadSpeedDistributionperhour(DateTime sd, DateTime ed, string vehicleid)
        {
            //这里不考虑按每一天进行合并，而是合并相同时间段的数据，不区分哪一天
            var speeddistributionlist = await Task.Run(() => _ISpeedDistribution_ACC_DAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId== vehicleid).AsNoTracking().GroupBy(x => 
            x.Datadate.Value.Hour).Select(x => new
            {
                Hour = x.Key,
                Distance = x.Sum(a => a._010) + x.Sum(a => a._100110) + x.Sum(a => a._1020) + x.Sum(a => a._110120) + x.Sum(a => a._2030) + x.Sum(a => a._3040)
                + x.Sum(a => a._4050) + x.Sum(a => a._5060) + x.Sum(a => a._6070) + x.Sum(a => a._7080) + x.Sum(a => a._8090) + x.Sum(a => a._90100)
            }));


            return speeddistributionlist;
        }

        //专门返回当天里程和累积里程
        public async Task<List<double>> LoadTextRecord(DateTime sd, DateTime ed, string vehicleid)
        {
            List<double> list = new List<double>();
            var currentmile = await Task.Run(() => _ISpeedDistribution_ACC_DAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).AsNoTracking().
            Sum(a=>a._010+a._1020+a._2030+a._3040+a._4050+a._5060+a._6070+a._7080+a._8090+a._90100+a._100110+a._110120+a.Above120));

            var accumtmile = await Task.Run(() => _ISpeedDistribution_ACC_DAL.LoadEntities(a =>  a.Datadate <= ed && a.VehicleId == vehicleid).AsNoTracking().
 Sum(a => a._010 + a._1020 + a._2030 + a._3040 + a._4050 + a._5060 + a._6070 + a._7080 + a._8090 + a._90100 + a._100110 + a._110120 + a.Above120));

            list.Add((double)currentmile);
            list.Add((double)accumtmile);
            return list;
        }

    }
}
