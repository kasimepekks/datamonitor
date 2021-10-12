using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL.SH_ADF0979IDAL
{
    public interface IRealTimeI_ACC_IDAL : IBaseDAL<RealtimeTempdataAcc>
    {
        
        //List<ShAdf0979RealtimeTempdataAcc> ReadOneFileForRealTimeAccReturn(string filefullpath, string filename, out List<double> speed, out List<double> brake);
    }
}
