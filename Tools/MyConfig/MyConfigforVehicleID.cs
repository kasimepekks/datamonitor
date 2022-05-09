using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.MyConfig
{
    public struct VehicleIDPara
    {
        public  string[] channels;
        public int Pointsperfile;
        public  double Wheelbaselower;
        public  double Wheelbaseupper;
        public  double BmupZeroStandard;
        public  double AccValueGap;
        public  int AccTimeGap;
        public  int BumpTimeGap;
        public  double BrakeZeroStandard;
        public  int BrakeLastingPoints;
        public  double SteeringZeroStandard;
        public  double SteeringLastingPoints;
        public  int Reductiontimesforimport;
        public  int ThrottleZeroStandard;
        public int ThrottleLastingPoints;
        public  int BumpMaxSpeed;
    }
    public static class MyConfigforVehicleID
    {
        private static IConfiguration configuration;
        private static string Vehicleid;
        public static int SampleRate;
        public static int reductiontimesforsampling;
        public static int ReductiontimesforGPS;
        //先读取配置里所有的车辆号
        static MyConfigforVehicleID()
        {
            configuration = new ConfigurationBuilder().AddJsonFile("vehicleidappsettings.json", optional: true, reloadOnChange: true).Build();
            Vehicleid = configuration["Global:VehicleID"];
            SampleRate = Convert.ToInt32(configuration["Global:SampleRate"]);
            reductiontimesforsampling = Convert.ToInt32(configuration["Global:Reductiontimesforsampling"]);
            ReductiontimesforGPS = Convert.ToInt32(configuration["Global:ReductiontimesforGPS"]);
        }

        //再根据传进来的车辆号来赋值给VehicleIDPara结构体并返回
        public static VehicleIDPara GetVehicleConfiguration(string vehicleid)
        {
            VehicleIDPara vehicleIDPara=new VehicleIDPara();
            if (Vehicleid.Contains(vehicleid))
            {
                vehicleIDPara.channels = configuration[vehicleid + ":Channels"].Split(",");
                vehicleIDPara.Pointsperfile = Convert.ToInt32(configuration[vehicleid + ":Pointsperfile"]);
                vehicleIDPara.Wheelbaselower = Convert.ToDouble(configuration[vehicleid+":Wheelbaselower"]);
                vehicleIDPara.Wheelbaseupper = Convert.ToDouble(configuration[vehicleid+":Wheelbaseupper"]);
                vehicleIDPara.BmupZeroStandard = Convert.ToDouble(configuration[vehicleid+":BmupZeroStandard"]);
                vehicleIDPara.AccValueGap = Convert.ToDouble(configuration[vehicleid+":AccValueGap"]);
                vehicleIDPara.AccTimeGap = Convert.ToInt32(configuration[vehicleid+":AccTimeGap"]);
                vehicleIDPara.BumpTimeGap = Convert.ToInt32(configuration[vehicleid+":BumpTimeGap"]);
                vehicleIDPara.BrakeZeroStandard = Convert.ToDouble(configuration[vehicleid+":BrakeZeroStandard"]);
                vehicleIDPara.BrakeLastingPoints = Convert.ToInt32(configuration[vehicleid+":BrakeLastingPoints"]);
                vehicleIDPara.SteeringZeroStandard = Convert.ToDouble(configuration[vehicleid+":SteeringZeroStandard"]);
                vehicleIDPara.SteeringLastingPoints = Convert.ToInt32(configuration[vehicleid+":SteeringLastingPoints"]);
                vehicleIDPara.Reductiontimesforimport= Convert.ToInt32(configuration[vehicleid + ":Reductiontimesforimport"]);
                vehicleIDPara.BumpMaxSpeed = Convert.ToInt32(configuration[vehicleid + ":BumpMaxSpeed"]);
                vehicleIDPara.ThrottleZeroStandard=Convert.ToInt32(configuration[vehicleid + ":ThrottleZeroStandard"]);
                vehicleIDPara.ThrottleLastingPoints = Convert.ToInt32(configuration[vehicleid + ":ThrottleLastingPoints"]);
            }
           
            return vehicleIDPara;
        }
    }
}
