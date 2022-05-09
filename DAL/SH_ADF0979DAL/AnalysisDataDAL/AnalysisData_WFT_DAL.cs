using IDAL.SH_ADF0979IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;
using Tools.AddDamage;
using Tools.MyConfig;

namespace DAL.SH_ADF0979DAL
{
    public class AnalysisData_WFT_DAL : BaseDAL<SatictisAnalysisdataWft>, IAnalysisData_WFT_IDAL
    {
        public AnalysisData_WFT_DAL(datawatchContext DB) : base(DB)
        {

        }
       
        public async Task<bool> ReadandMergeWFTDataperHalfHour(string filepath,string vehicleid)
        {
            FileInfo[] filelist = FileOperator.Isfileexist(filepath);//获得指定文件下的所有csv文件
            Encoding encoding = Encoding.Default;
            bool can = false;
            await Task.Run(() => {

                if (filelist.Length > 0)
                {
                    List<SatictisAnalysisdataWft> List = new List<SatictisAnalysisdataWft>();
                    var wftmysqllist = _DB.Set<SatictisAnalysisdataWft>().Select(a => a.Id).ToList();

                    foreach (var file in filelist)
                    {
                        if (file.Length != 0)
                        {
                            string[] filestring = file.Name.Split('-');//把文件名每隔“-”拿出来放在这个string数组里
                            string date = filestring[0].Replace("_", "-");//date是拿出来的日期，如“2021-07-03”
                            string oldtime = filestring[1];//oldtime是拿出来的时间，如“10_07_03”
                            string[] timestring = oldtime.Split('_');//把小时，分钟，秒数放到这个数组里
                            string newminute = (int.Parse(timestring[1]) / 30 * 30).ToString();//把所有的分钟改为小于30分的都是0分，大于30分都是30分
                            string newtime = timestring[0] + ":" + newminute + ":" + "00";

                            string datetime = date + " " + newtime;
                            var datestart = Convert.ToDateTime(datetime);


                            string name = file.Name.Split('.')[0];


                            FileStream fs = new FileStream(file.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                            StreamReader sr = new StreamReader(fs, encoding);
                            DataTable dt = new DataTable();
                            string strLine = "";
                            //记录每行记录中的各字段内容
                            string[] aryLine = null;
                            string[] tableHead = null;


                            //标示列数
                            int columnCount = 0;
                            //标示是否是读取的第一行
                            bool IsFirst = true;
                            //逐行读取CSV中的数
                            while ((strLine = sr.ReadLine()) != null)
                            {

                                if (IsFirst == true)
                                {
                                    tableHead = strLine.Split(',');
                                    IsFirst = false;
                                    columnCount = tableHead.Length;

                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        var t = tableHead[i].Replace("_", "");
                                        tableHead[i] = t.Replace(" ", "");
                                    }

                                    //创建列
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        DataColumn dc = new DataColumn(tableHead[i]);
                                        dt.Columns.Add(dc);
                                    }
                                }
                                else
                                {
                                    aryLine = strLine.Split(',');
                                    DataRow dr = dt.NewRow();
                                    for (int j = 0; j < columnCount; j++)
                                    {
                                        dr[j] = aryLine[j];
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                            sr.Close();
                            sr.Dispose();

                            fs.Close();
                            fs.Dispose();
                            if (aryLine != null && aryLine.Length > 0)
                            {

                                for (int l = 0; l < columnCount - 1; l++)
                                {

                                    SatictisAnalysisdataWft entity = new SatictisAnalysisdataWft();

                                    entity.Id = vehicleid + "-" + name + "-WFT-" + l.ToString();
                                    entity.VehicleId = vehicleid;
                                    entity.Filename = name;
                                    entity.Datadate = Convert.ToDateTime(datetime);
                                    entity.Chantitle = tableHead[l + 1];

                                    //注意这里由于计算的列的格式是string类型，用max或min计算会出问题，所以必须先转换再求maxmin


                                    entity.Max = dt.AsEnumerable().Max(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));
                                    entity.Min = dt.AsEnumerable().Min(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));


                                    List<double> damage = dt.AsEnumerable().Select(a => Convert.ToDouble(a.Field<string>(tableHead[l + 1]))).ToList();
                                    entity.Damage = GetDamageFromList.GetDamage(damage);

                                    List<string> lst = (from d in dt.AsEnumerable() select d.Field<string>(tableHead[l + 1])).ToList();
                                    double t = 0;
                                    int n = lst.Count;
                                    foreach (var data in lst)
                                    {

                                        t += Convert.ToDouble(data) * Convert.ToDouble(data);
                                    }


                                    entity.Rms = System.Math.Sqrt(t / n);

                                    entity.Range = entity.Max - entity.Min;



                                    List.Add(entity);


                                }



                            }
                        }

                        else
                        {
                            continue;
                        }

                    }

                    var ListperHalfHour = List.GroupBy(x => new
                    {
                        x.Chantitle,
                        x.Datadate.Value.Year,
                        x.Datadate.Value.Month,
                        x.Datadate.Value.Day,
                        x.Datadate.Value.Hour,
                        x.Datadate.Value.Minute
                    }
                   ).Select(x => new
                   {
                       Id = x.Min(a => a.Id),
                       VehicleID = vehicleid,
                       Filename = x.Min(a => a.Filename),
                       Datadate = x.Min(a => a.Datadate),
                       x.Key.Chantitle,

                       Max = x.Max(a => a.Max),
                       Min = x.Min(a => a.Min),
                       Range = x.Max(a => a.Max) - x.Min(a => a.Min),
                       Rms = x.Sum(a => a.Rms) * Math.Sqrt(x.Count()),

                       Damage = Math.Round((double)x.Sum(a => a.Damage), 0),

                   }).OrderBy(b => b.Chantitle).ThenBy(b => b.Datadate).ToList();

                    //var ListperHalfHourConvert= ListperHalfHour.OfType<ShAdf0979SatictisAnalysisdataAcc>().ToList();

                    List<SatictisAnalysisdataWft> ListperHalfHourConvert = ListperHalfHour.Select(a => new SatictisAnalysisdataWft
                    {
                        Id = a.Id,
                        VehicleId = a.VehicleID,
                        Filename = a.Filename,
                        Datadate = a.Datadate,
                        Chantitle = a.Chantitle,
                        Max = a.Max,
                        Min = a.Min,
                        Range = a.Range,
                        Rms = a.Rms,
                        Damage = a.Damage

                    }).ToList();

                    for (int i = ListperHalfHourConvert.Count - 1; i >= 0; i--)
                    {
                        if (wftmysqllist.Contains(ListperHalfHourConvert[i].Id))
                        {
                            ListperHalfHourConvert.Remove(ListperHalfHourConvert[i]);
                        }
                    }


                    _DB.BulkInsert(ListperHalfHourConvert);

                    //this.CurrentDBSession.SaveChanges();
                    _DB.SaveChanges();
                    can = true;

                }

            });
            

            return can;


        }
    }
}
