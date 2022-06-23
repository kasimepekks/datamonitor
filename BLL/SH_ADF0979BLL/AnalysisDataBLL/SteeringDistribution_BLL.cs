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
    public class SteeringDistribution_BLL : BaseBLL<Streeringrecognition>, ISteeringDistribution_IBLL
    {
        private readonly ISteeringDistribution_IDAL _SteeringDistributionDAL;
        //private readonly datawatchContext _DB;
        public SteeringDistribution_BLL(ISteeringDistribution_IDAL SteeringDistributionDAL)
        {
            this._SteeringDistributionDAL = SteeringDistributionDAL;
            base.CurrentDal = SteeringDistributionDAL;
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._SteeringDistributionDAL;
        }
        //返回转向强度的数据给前端
        public async Task<IQueryable> LoadSteeringDistribution(DateTime sd, DateTime ed,string vehicleid)
        {
            var steeringdistributionlist = await Task.Run(()=>_SteeringDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).Select(a => new { 
            a.StrgWhlAng,a.AngularAcc,a.Speed,a.SteeringAcc,a.SteeringDirection
            }).ToList());
            return steeringdistributionlist.AsQueryable();
        }
        //返回转向次数
        public async Task<int> GetSteeringCount(DateTime sd, DateTime ed, string vehicleid)
        {
            var steeringcount = await Task.Run(() => _SteeringDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId==vehicleid).Select(a => a.SteeringAcc).ToList().Count());
            return steeringcount;
        }

    }
}
