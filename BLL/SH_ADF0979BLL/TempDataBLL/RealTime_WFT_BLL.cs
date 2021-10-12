using DAL.SH_ADF0979;
using IBLL.SH_ADF0979IBLL;
using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BLL.SH_ADF0979BLL
{
   public class RealTime_WFT_BLL:BaseBLL<RealtimeTempdataWft>, IRealTime_WFT_IBLL
    {
        //private readonly IRealTimeI_WFT_IDAL _RealTime_WFT_Dal;
      
        //private readonly datawatchContext _DB;
        public RealTime_WFT_BLL(IRealTimeI_WFT_IDAL RealTime_WFT_Dal)
        {
            //this._RealTime_WFT_Dal = RealTime_WFT_Dal;
            base.CurrentDal = RealTime_WFT_Dal;
            //_DB = DB;
        }

        public override void SetCurrentDal()
        {
            
        }

     

    }
}
