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
    public class BrakeDistribution_BLL : BaseBLL<Brakerecognition>, IBrakeDistribution_IBLL
    {
        private readonly IBrakeDistribution_IDAL _BrakeDistributionDAL;
        //private readonly datawatchContext _DB;
        public BrakeDistribution_BLL(IBrakeDistribution_IDAL BrakeDistributionDAL)
        {
            this._BrakeDistributionDAL = BrakeDistributionDAL;
            base.CurrentDal = BrakeDistributionDAL;
            //_DB = DB;
        }
        public override void SetCurrentDal()
        {
            base.CurrentDal = this._BrakeDistributionDAL;
        }
        //返回刹车强度的数据给前端
        public async Task<List<double>> LoadBrakeDistribution(DateTime sd, DateTime ed,string vehicleid)
        {
            var brakedistributionlist = await Task.Run(()=>_BrakeDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).Select(a => a.BrakeAcc).ToList());
            return brakedistributionlist;
        }
        //返回刹车次数
        public async Task<int> GetBrakeCount(DateTime sd, DateTime ed, string vehicleid)
        {
            var brakecount = await Task.Run(() => _BrakeDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId==vehicleid).Select(a => a.BrakeAcc).ToList().Count());
            return brakecount;
        }

    }
}
