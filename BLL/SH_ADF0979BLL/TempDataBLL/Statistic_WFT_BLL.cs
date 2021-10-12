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
   public class Statistic_WFT_BLL : BaseBLL<SatictisData>, IStatistic_WFT_IBLL
    {
        //private readonly IStatistic_WFT_IDAL _Statistic_WFT_DAL;
      
        public Statistic_WFT_BLL(IStatistic_WFT_IDAL Statistic_WFT_DAL)
        {
            //this._Statistic_WFT_DAL = Statistic_WFT_DAL;
            base.CurrentDal = Statistic_WFT_DAL;
        }

        public override void SetCurrentDal()
        {
            //base.CurrentDal = this._Statistic_WFT_DAL;
        }


      

    }
}
