using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tools.AddDistance
{
   public class AddDistanceforAnalysisData
    {
        /// <summary>
        /// 添加distance字段到每一个加速度的csv文件中
        /// </summary>
        //public static bool AddDistanceforAnalysisDataAcc(string filepath)
        //{

           
        //    FileInfo[] filelist= FileOperator.Isfileexist(filepath);//获得指定文件下的所有csv文件
        //    bool Distanceexist = false;
        //    Encoding encoding = Encoding.Default;

        //    //提取每一个CSV文件进行操作，先读取speed和time并分别放入List数组中，再进行distance计算并把distance数据添加到原csv文件中
        //    foreach (var file in filelist)
        //    {
        //        if (file.Length != 0)
        //        {
        //            string strLine = "";
        //            //记录每行记录中的各字段内容
        //            string[] aryLine = null;
        //            string[] tableHead = null;


        //            ////标示列数
        //            //int columnCount = 0;

        //            //标示是否是读取的第一行,如果没有表头，则为false
        //            bool IsFirst = true;
        //            //逐行读取CSV中的数据


        //            List<double> speedlist = new List<double>();
        //            List<double> timelist = new List<double>();
        //            List<double> singledistance = new List<double>();
        //            using (FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.Read))
        //            {
        //                StreamReader sr = new StreamReader(fs, encoding);
                                                
        //                while ((strLine = sr.ReadLine()) != null)
        //                {

        //                    if (IsFirst == true)
        //                    {
        //                        tableHead = strLine.Split(',');
        //                        IsFirst = false;

        //                    }
        //                    else
        //                    {

        //                        aryLine = strLine.Split(',');

        //                        speedlist.Add(Convert.ToDouble(aryLine[1]));
        //                        timelist.Add(Convert.ToDouble(aryLine[0]));

        //                    }


        //                }
        //                sr.Close();
        //                fs.Close();
        //            }
        //            if (!tableHead.Contains("AccumulatedDistance"))
        //            {
        //                var accumulateddistance = AddDistanceList.AddDistanceToCSV(speedlist, timelist, out singledistance);

        //                var lines = File.ReadLines(file.FullName).Select((line, index) => index == 0
        //                            ? line + ",AccumulatedDistance,SingleDistance"
        //                            : line + "," + accumulateddistance[index - 1].ToString() + "," + singledistance[index - 1].ToString())
        //                            .ToList();

        //                File.WriteAllLines(file.FullName, lines);
                       

        //            }

        //            else
        //            {
        //                //如果已经添加过了就进入下一循环
        //                continue;

        //            }



        //        }


                


        //    }
        //    return Distanceexist;
           
           

        //}
    }
}
