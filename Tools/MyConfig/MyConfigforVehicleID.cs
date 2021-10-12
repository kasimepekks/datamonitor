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

        public static int SteeringLastingPoints;

        static MyConfigforVehicleID()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("vehicleidappsettings.json", optional: true, reloadOnChange: true).Build();
            VehicleID=configuration["ADF0979:VehicleID"];
            Wheelbaselower = Convert.ToDouble(configuration["ADF0979:Wheelbaselower"]);
            Wheelbaseupper = Convert.ToDouble(configuration["ADF0979:Wheelbaseupper"]);
            BmupZeroStandard = Convert.ToDouble(configuration["ADF0979:BmupZeroStandard"]);
            AccValueGap = Convert.ToDouble(configuration["ADF0979:AccValueGap"]);
            AccTimeGap = Convert.ToInt32(configuration["ADF0979:AccTimeGap"]);
            BumpTimeGap = Convert.ToInt32(configuration["ADF0979:BumpTimeGap"]);
            BrakeZeroStandard = Convert.ToDouble(configuration["ADF0979:BrakeZeroStandard"]);
            BrakeLastingPoints = Convert.ToInt32(configuration["ADF0979:BrakeLastingPoints"]);
            SteeringZeroStandard = Convert.ToDouble(configuration["ADF0979:SteeringZeroStandard"]);
            SteeringLastingPoints = Convert.ToInt32(configuration["ADF0979:SteeringLastingPoints"]);

        }
    }
}
