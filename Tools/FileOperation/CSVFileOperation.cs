using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tools.Cash;

namespace Tools.FileOperation
{
    public struct CsvFileReturnAllList<T> where T : class, new()
    {
        public List<T> TList;//记录所有的csv数据，采样率应该是512
        public List<T> TListReSampling;//对TList重采样，采样率降低了20倍的结果，目的是降低传输数据的数量，以使前端获取数据时带宽够用
        public List<double> Speed;
        public List<double> Brake;
        public List<double> Lat;
        public List<double> Lon;
        public List<double> StrgWhlAng;
        public string name;
        public List<SatictisData> STList;
        public double sdistance;
    }
      
    public class CSVFileOperation<T> where T : class, new()
    {
        /// <summary>
        /// 这里的泛型T主要是指input还是result文件的类型
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static async Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename)
        {
            var returnlist = new CsvFileReturnAllList<T>()
            {
                TList = new List<T>(),
                STList = new List<SatictisData>(),
                TListReSampling = new List<T>(),
                Speed = new List<double>(),
                Lat = new List<double>(),
                Lon = new List<double>(),
                StrgWhlAng= new List<double>(),
                name="",
                sdistance=0
            };

            try
            {
                if(!FileOperator.FileIsUsed(filefullpath))
                {
                    //Console.WriteLine("此文件没有被占用");
                    await Task.Run(() => {
                        Encoding encoding = Encoding.Default;
                        returnlist.name = filename.Split('.')[0];
                        //string starttime = name.Split('-')[1];
                        using FileStream fs = new FileStream(filefullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                        StreamReader sr = new StreamReader(fs, encoding);
                        DataTable dt = new DataTable();
                        string strLine = "";
                        //记录每行记录中的各字段内容
                        string[] aryLine = null;
                        string[] tableHead = null;
                        int columnCount = 0;
                        ////标示列数
                        //int columnCount = 0;

                        //标示是否是读取的第一行
                        bool IsFirst = true;
                        //逐行读取CSV中的数据

                        //_DB.Database.ExecuteSqlRaw("TRUNCATE TABLE sh_adf0979_realtime_tempdata_acc");

                        //用一个泛型缓存来存储所有属性，提高性能
                        PropertyInfo[] props = ReadTimedomainCash<T>.GetProps();

                        //CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;

                        while ((strLine = sr.ReadLine()) != null)
                        {

                            if (IsFirst == true)
                            {
                                tableHead = strLine.Split(',');
                                IsFirst = false;
                                columnCount = tableHead.Length;
                                //把csv里的列名里所有的下划线,N和空格去除，与我的类名保持一致
                                for (int i = 0; i < columnCount; i++)
                                {
                                    var t = tableHead[i].Replace("_", "").Replace("N", "");
                                    tableHead[i] = t.Replace(" ", "");
                                }
                                for (int i = 0; i < columnCount; i++)
                                {
                                    //把重复的列名改为唯一的名称，否则会出错
                                    if (tableHead[i].Contains("blank"))
                                    {
                                        tableHead[i] = tableHead[i] + i;
                                    }
                                    DataColumn dc = new DataColumn(tableHead[i]);


                                    dt.Columns.Add(dc);

                                }
                            }
                            else
                            {

                                aryLine = strLine.Split(',');


                                T titem = new T();


                                //只要列名与类里的属性名一致就能提取出来，所以类的属性尽量全，只要包含csv里的列名，就可以读取
                                foreach (var p in props)
                                {
                                    if (tableHead.ToList().IndexOf(p.Name) != -1)
                                    {
                                        p.SetValue(titem, Convert.ToDouble(aryLine[tableHead.ToList().IndexOf(p.Name)]));
                                    }

                                }
                                if (aryLine[0] != "")//防止读到所有的空行
                                {
                                    DataRow dr = dt.NewRow();
                                    for (int j = 0; j < aryLine.Length; j++)
                                    {
                                        dr[j] = aryLine[j];
                                    }
                                    dt.Rows.Add(dr);
                                }


                                returnlist.TList.Add(titem);

                            }


                        }
                        //重采样，降低10倍
                        for (int i = 0; i < returnlist.TList.Count / MyConfig.MyConfigforVehicleID.reductiontimesforsampling; i++)
                        {
                            returnlist.TListReSampling.Add(returnlist.TList.Skip(i * MyConfig.MyConfigforVehicleID.reductiontimesforsampling).Take(1).FirstOrDefault());
                        }
                        sr.Close();
                        sr.Dispose();
                        fs.Close();
                        fs.Dispose();

                        if (ReadTimedomainCash<T>.ACCorWFT())
                        {
                            //把list<T>转为List<TempdataAccBase>父类，父类有速度，GPS和刹车
                            var ACCList = returnlist.TList.Cast<TempdataAccBase>();
                            var somedatalist = ACCList.Where(a => a.Time == 0 || a.Time == 1 || a.Time == 2
                                || a.Time == 3 || a.Time == 4 || a.Time == 5 || a.Time == 6 || a.Time == 7 || a.Time == 8 || a.Time == ACCList.LastOrDefault().Time).Select(b => new
                                {
                                    b.Speed,
                                    b.Lat,
                                    b.Lon,
                                    b.Spd,
                                    b.Brake,
                                    b.StrgWhlAng

                                }).ToList();
                            //判断CSV里有哪个速度，有哪个就用哪个，如果都有或者都没有就用Spd
                            if (tableHead.Contains("Speed") && !tableHead.Contains("Spd"))
                            {
                                returnlist.Speed = somedatalist.Select(a => a.Speed).ToList();
                            }
                            else
                            {
                                returnlist.Speed = somedatalist.Select(a => a.Spd).ToList();
                            }

                            returnlist.Brake = somedatalist.Select(a => a.Brake).ToList();
                            returnlist.Lat = somedatalist.Select(a => a.Lat).ToList();
                            returnlist.Lon = somedatalist.Select(a => a.Lon).ToList();
                            returnlist.StrgWhlAng = somedatalist.Select(a => a.StrgWhlAng).ToList();
                        }
                       

                        if (aryLine != null && aryLine.Length > 0)
                        {


                            //_DB.Database.ExecuteSqlRaw("TRUNCATE TABLE sh_adf0979_satictis_tempdata_acc");
                            for (int l = 0; l < aryLine.Length - 1; l++)
                            {

                                SatictisData entity = new SatictisData();

                                //entity.Id = name + "-" + l.ToString();
                                //entity.Time = name;
                                entity.Chantitle = tableHead[l + 1];

                                //注意这里由于计算的列的格式是string类型，用max或min计算会出问题，所以必须先转换再求maxmin
                                //entity.max = dt.Columns[0].Table.AsEnumerable().Select(cols => cols.Field<double>(dt.Columns[0].ColumnName)).Max();
                                entity.Max = dt.AsEnumerable().Max(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));
                                entity.Min = dt.AsEnumerable().Min(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));

                                List<string> lst = (from d in dt.AsEnumerable() select d.Field<string>(tableHead[l + 1])).ToList();
                                double t = 0;
                                int n = lst.Count;
                                foreach (var data in lst)
                                {

                                    t += Convert.ToDouble(data) * Convert.ToDouble(data);
                                }


                                entity.Rms = System.Math.Sqrt(t / n);
                                //    entity.min = dt.AsEnumerable().Select(t => t.Field<double>(tableHead[l + 1])).Min();
                                entity.Range = entity.Max - entity.Min;
                                returnlist.STList.Add(entity);

                            }
                            if (tableHead.Contains("Speed") && !tableHead.Contains("Spd"))
                            {
                                var speed = returnlist.STList.Where(a => a.Chantitle == "Speed").Select(a => a.Rms).ToList().FirstOrDefault();//选用speed的rms来作为计算里程的参数
                                returnlist.sdistance = speed * 10 / 3600;

                            }
                            else
                            {
                                var speed = returnlist.STList.Where(a => a.Chantitle == "Spd").Select(a => a.Rms).ToList().FirstOrDefault();//选用speed的rms来作为计算里程的参数
                                returnlist.sdistance = speed * 10 / 3600;

                            }

                        }
                        else
                        {
                            returnlist.sdistance = 0;

                        }
                    });
                    return returnlist;
                }
                else
                {
                    Console.WriteLine("文件:"+ filename+"被占用。 "+DateTime.Now);
                    return returnlist;
                }
               
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

      
    }


}
