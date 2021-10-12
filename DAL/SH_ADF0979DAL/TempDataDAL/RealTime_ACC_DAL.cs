using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.SH_ADF0979
{
   public class RealTime_ACC_DAL : BaseDAL<RealtimeTempdataAcc>, IRealTimeI_ACC_IDAL
    {
        
        public RealTime_ACC_DAL(datawatchContext DB) :base(DB){
           
        }

      
    }
}
