using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLDA_VehicleData_Watch.Models
{
    public class LoginClass
    {
        public static SysAuthority Login(string id, string pwd)
        {

            datawatchContext saicdb = new datawatchContext();
            SysAuthority sys_Authority = saicdb.SysAuthorities.FirstOrDefault(p => p.LoginName == id && p.Password == pwd);
            return sys_Authority;
        }
    }
}
