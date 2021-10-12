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
   public class Statistic_ACC_BLL : BaseBLL<SatictisData>, IStatistic_ACC_IBLL
    {
        //private readonly IStatistic_ACC_IDAL _Statistic_ACC_DAL;
      
        public Statistic_ACC_BLL(IStatistic_ACC_IDAL Statistic_ACC_DAL)
        {
            //this._Statistic_ACC_DAL = Statistic_ACC_DAL;
            base.CurrentDal = Statistic_ACC_DAL;
        }

        public override void SetCurrentDal()
        {
            //base.CurrentDal = this._Statistic_ACC_DAL;
        }


        
    }
}
