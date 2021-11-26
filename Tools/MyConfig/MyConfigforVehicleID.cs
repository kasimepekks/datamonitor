using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.MyConfig
{
    public static class MyConfigforVehicleID
    {
        public static string VehicleID;
        public static double Wheelbaselower;
        public static double Wheelbaseupper;
        public static double BmupZeroStandard;
        public static double AccValueGap;
        public static int AccTimeGap;
        public static int BumpTimeGap;
        public static double BrakeZeroStandard;

        public static int BrakeLastingPoints;

        public static double SteeringZeroStandard;

        public static double SteeringLastingPoints;
        public static string accessKeyId;
        public static string accessKeySecret;
        public static string endpoint;
        public static string drive;
   

        static MyConfigforVehicleID()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("vehicleidappsettings.json", optional: true, reloadOnChange: true).Build();
            VehicleID=configuration["VehicleSetup:VehicleID"];
            Wheelbaselower = Convert.ToDouble(configuration["VehicleSetup:Wheelbaselower"]);
            Wheelbaseupper = Convert.ToDouble(configuration["VehicleSetup:Wheelbaseupper"]);
            BmupZeroStandard = Convert.ToDouble(configuration["VehicleSetup:BmupZeroStandard"]);
            AccValueGap = Convert.ToDouble(configuration["VehicleSetup:AccValueGap"]);
            AccTimeGap = Convert.ToInt32(configuration["VehicleSetup:AccTimeGap"]);
            BumpTimeGap = Convert.ToInt32(configuration["VehicleSetup:BumpTimeGap"]);
            BrakeZeroStandard = Convert.ToDouble(configuration["VehicleSetup:BrakeZeroStandard"]);
            BrakeLastingPoints = Convert.ToInt32(configuration["VehicleSetup:BrakeLastingPoints"]);
            SteeringZeroStandard = Convert.ToDouble(configuration["VehicleSetup:SteeringZeroStandard"]);
            SteeringLastingPoints = Convert.ToInt32(configuration["VehicleSetup:SteeringLastingPoints"]);
            accessKeyId = configuration["OSSSetup:accessKeyId"];
            accessKeySecret = configuration["OSSSetup:accessKeySecret"];
            endpoint = configuration["OSSSetup:endpoint"];
            drive = configuration["OSSSetup:drive"];
        }
    }
}
