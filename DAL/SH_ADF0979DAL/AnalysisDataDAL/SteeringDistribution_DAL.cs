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
    public class SteeringDistribution_DAL : BaseDAL<Streeringrecognition>, ISteeringDistribution_IDAL
    {
        public SteeringDistribution_DAL(datawatchContext DB) : base(DB)
        {

        }
       
    }
}
