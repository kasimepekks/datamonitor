using System;
using System.Collections.Generic;

#nullable disable

namespace MysqlforDataWatch
{
    public partial class SysAuthority
    {
        public uint Id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public int AuthorityKey { get; set; }
    }
}
