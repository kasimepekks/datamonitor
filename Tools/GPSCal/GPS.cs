using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.MyConfig;

namespace Tools.GPSCal
{
   public class GPS
   {
        private static  double PI = 3.14159265;
        private static  double EARTH_RADIUS = 6378137;
        private static  double RAD = Math.PI / 180.0;

        /// <summary>
        /// 获取以某个经纬度点为圆心，给定半径R的经纬度范围
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="raidus"></param>
        /// <returns></returns>
        public static double[] getAround(double lat, double lon, int raidus)
        {

            double latitude = lat;
            double longitude = lon;

            double degree = (24901 * 1609) / 360.0;
            double raidusMile = raidus;

            double dpmLat = 1 / degree;
            double radiusLat = dpmLat * raidusMile;
            double minLat = latitude - radiusLat;
            double maxLat = latitude + radiusLat;

            double mpdLng = degree * Math.Cos(latitude * (PI / 180));
            double dpmLng = 1 / mpdLng;
            double radiusLng = dpmLng * raidusMile;
            double minLng = longitude - radiusLng;
            double maxLng = longitude + radiusLng;
            return new double[] { minLat, minLng, maxLat, maxLng };
        }
        /// <summary>
        /// 获取两个经纬度的距离
        /// </summary>
        /// <param name="lng1"></param>
        /// <param name="lat1"></param>
        /// <param name="lng2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        public static double getDistance(double lng1, double lat1, double lng2, double lat2)
        {
            double radLat1 = lat1 * RAD;
            double radLat2 = lat2 * RAD;
            double a = radLat1 - radLat2;
            double b = (lng1 - lng2) * RAD;
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }

        public static double GetAngel(double lat1, double lng1, double lat2, double lng2)
        {
            double x1 = lng1;
            double y1 = lat1;
            double x2 = lng2;
            double y2 = lat2;
            double pi = Math.PI;
            double w1 = y1 / 180 * pi;
            double j1 = x1 / 180 * pi;
            double w2 = y2 / 180 * pi;
            double j2 = x2 / 180 * pi;
            double ret;
            if (j1 == j2)
            {
                if (w1 > w2) return 270; //北半球的情况，南半球忽略
                else if (w1 < w2) return 90;
                else return -1;//位置完全相同
            }
            ret = 4 * Math.Pow(Math.Sin((w1 - w2) / 2), 2) - Math.Pow(Math.Sin((j1 - j2) / 2) * (Math.Cos(w1) - Math.Cos(w2)), 2);
            ret = Math.Sqrt(ret);
            double temp = (Math.Sin(Math.Abs(j1 - j2) / 2) * (Math.Cos(w1) + Math.Cos(w2)));
            ret = ret / temp;
            ret = Math.Atan(ret) / pi * 180;
            if (j1 > j2) // 1为参考点坐标
            {
                if (w1 > w2) ret += 180;
                else ret = 180 - ret;
            }
            else if (w1 > w2) ret = 360 - ret;
            return ret;
        }

        public static string GetDirection(double lat1, double lng1, double lat2, double lng2)
        {
            double jiaodu = GetAngel(lat1, lng1, lat2, lng2);
            if ((jiaodu <= 10) || (jiaodu > 350)) return "东";
            if ((jiaodu > 10) && (jiaodu <= 80)) return "东北";
            if ((jiaodu > 80) && (jiaodu <= 100)) return "北";
            if ((jiaodu > 100) && (jiaodu <= 170)) return "西北";
            if ((jiaodu > 170) && (jiaodu <= 190)) return "西";
            if ((jiaodu > 190) && (jiaodu <= 260)) return "西南";
            if ((jiaodu > 260) && (jiaodu <= 280)) return "南";
            if ((jiaodu > 280) && (jiaodu <= 350)) return "东南";
            return string.Empty;

        }
        
        /// <summary>
        /// 经纬度数据降采样,第一次降采样
        /// </summary>
        /// <param name="originallist"></param>
        /// <returns></returns>
        public static List<double> GPSResampling(List<double> originallist)
        {
            int resamplecount = 0;
            List<double> result = new List<double>();
            for (int i = 0; i < originallist.Count; i++)
            {
                resamplecount += 1;
                if(resamplecount== MyConfigforVehicleID.ReductiontimesforGPS)
                {
                    result.Add(originallist[i]);
                    resamplecount = 0;
                }
            }
            return result;
        }

    }
}
