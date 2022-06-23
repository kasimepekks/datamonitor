using IDAL.SH_ADF0979IDAL;

using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Tools;
using Tools.AddDistance;
using Tools.MyConfig;

namespace DAL.SH_ADF0979DAL
{
    public class ThrottleDistribution_DAL : BaseDAL<Throttlerecognition>, IThrottleDistribution_IDAL
    {
        public ThrottleDistribution_DAL(datawatchContext DB) : base(DB)
        {

        }
       
    }
}
