using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL.SH_ADF0979
{
   public class RealTime_WFT_DAL : BaseDAL<RealtimeTempdataWft>, IRealTimeI_WFT_IDAL
    {
        
        public RealTime_WFT_DAL(datawatchContext DB) :base(DB){
           
        }

     
        
    }
}
