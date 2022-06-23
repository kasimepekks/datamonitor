using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Text;
using Tools.MyConfig;

namespace Tools.GPSCal
{
   public static class AddGPS
    {
        public static List<Gpsrecord> addgpslist(List<double> l11_Lat, List<double> l12_Lon, List<double> Speed, List<string> sqllist, string vehicleid, string name, string datetime, VehicleIDPara vehicleIDPara)
        {

            List<Gpsrecord> gpsrecordlist = new List<Gpsrecord>();
            if (vehicleIDPara.GPSImport == "true")
            {
                var lat = GPS.GPSResampling(l11_Lat);
                var lon = GPS.GPSResampling(l12_Lon);
                var speed = GPS.GPSResampling(Speed);
                for (int i = 0; i < lat.Count; i++)
                {
                    Gpsrecord gpsrecord = new Gpsrecord();
                    gpsrecord.Id = vehicleid + "-" + name + "-GPS-" + i.ToString();
                    gpsrecord.Filename = name;
                    gpsrecord.VehicleId = vehicleid;
                    gpsrecord.Datadate = Convert.ToDateTime(datetime);
                    gpsrecord.Lat = lat[i];
                    gpsrecord.Lon = lon[i];
                    gpsrecord.Speed = speed[i];
                    if (!sqllist.Contains(gpsrecord.Id))
                    {
                        gpsrecordlist.Add(gpsrecord);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
         
            return gpsrecordlist;
        }   
    }
}
