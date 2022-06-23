using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MysqlforDataWatch
{
  public static class MysqlConnectionConfiguration
    {
        public static string mysqlconnection;
        static MysqlConnectionConfiguration()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("mysqlconnection.json", optional: true, reloadOnChange: true).Build();
            mysqlconnection = configuration["ConnectionStrings:MySqlConnection"];
        }
    }
}
