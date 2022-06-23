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
    public class BumpDistribution_BLL : BaseBLL<Bumprecognition>, IBumpDistribution_IBLL
    {
        private readonly IBumpDistribution_IDAL _BumpDistributionDAL;
        //private readonly datawatchContext _DB;
        public BumpDistribution_BLL(IBumpDistribution_IDAL BumpDistributionDAL)
        {
            this._BumpDistributionDAL = BumpDistributionDAL;
            base.CurrentDal = BumpDistributionDAL;
            //_DB = DB;
        }

        public override void SetCurrentDal()
        {
            base.CurrentDal = this._BumpDistributionDAL;
        }

        public async Task<List<double>> LoadBumpDistribution(DateTime sd, DateTime ed, string vehicleid)
        {
            var bumpdistributionlist = await Task.Run(() => _BumpDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).Select(a => a.BumpAcc).ToList());
            return bumpdistributionlist;
        }
        

        public async Task<int> GetBumpCount(DateTime sd, DateTime ed, string vehicleid)
        {
            var bumpcount = await Task.Run(() => _BumpDistributionDAL.LoadEntities(a => a.Datadate >= sd && a.Datadate <= ed && a.VehicleId == vehicleid).Select(a => a.BumpAcc).ToList().Count());
            return bumpcount;
        }

    }
}
