using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SH_ADF0979BLL
{
    public class ThrottleDistribution_BLL : BaseBLL<Throttlerecognition>, IThrottleDistribution_IBLL
    {
        private readonly IThrottleDistribution_IDAL _ThrottleDistributionDAL;
        //private readonly datawatchContext _DB;
        public ThrottleDistribution_BLL(IThrottleDistribution_IDAL ThrottleDistributionDAL)
        {
            this._ThrottleDistributionDAL = ThrottleDistributionDAL;
            base.CurrentDal = ThrottleDistributionDAL;
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._ThrottleDistributionDAL;
        }
        //返回油门的数据给前端
        public async Task<IQueryable> LoadThrottleDistribution(DateTime sd, DateTime ed,string vehicleid)
        {
            var throttledistributionlist = await Task.Run(()=>_ThrottleDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).Select(a => new { a.ThrottleAcc,a.Accelerograph,a.Speed,a.LastingTime,a.Reverse}).ToList());
            return throttledistributionlist.AsQueryable();
        }
        //返回油门次数
        public async Task<int> GetThrottleCount(DateTime sd, DateTime ed, string vehicleid)
        {
            var throttlecount = await Task.Run(() => _ThrottleDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId==vehicleid).Select(a => a.ThrottleAcc).ToList().Count());
            return throttlecount;
        }

    }
}
