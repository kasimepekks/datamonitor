using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Tools.Cash;
using Tools.FileOperation;

namespace DAL
{
    public class BaseDAL<T> where T : class, new()
    {
        protected readonly datawatchContext _DB;
        public BaseDAL(datawatchContext db)
        {
            _DB = db;
        }
        public IQueryable<T> LoadEntities
          (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return _DB.Set<T>().Where<T>(whereLambda);
        }
       
        public bool DeleteEntity(T entity)
        {
            _DB.Entry<T>(entity).State = EntityState.Deleted;
            return true;
            //return DB.SaveChanges() > 0;
        }
        public bool EditEntity(T entity)
        {
            _DB.Entry<T>(entity).State = EntityState.Modified;
            return true;
            //return DB.SaveChanges() > 0;
        }
        public bool AddEntity(T entity)
        {
            _DB.Set<T>().Add(entity);
            //DB.SaveChanges();
            return true;
        }
        public bool SaveChanges()
        {
            return _DB.SaveChanges()>0;
         }

        /// <summary>
        /// 提供一个公用的方法读取csv数据（无论是input还是result数据），优点：不用再分开来写两个方法了
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <param name="filename"></param>
        /// <param name="name">给前端提供文件名</param>
        /// <param name="speed"></param>
        /// <param name="StrgWhlAng"></param>
        /// <returns></returns>
        //public List<T> ReadOneCsvFile(string filefullpath, string filename, out string name,out List<double> speed, out List<double> Brake, out List<double> Lat, out List<double> Lon)
        //{
            
        //    Encoding encoding = Encoding.Default;

        //     name = filename.Split('.')[0];
        //    //string starttime = name.Split('-')[1];
        //    using FileStream fs = new FileStream(filefullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
        //    StreamReader sr = new StreamReader(fs, encoding);

        //    string strLine = "";
        //    //记录每行记录中的各字段内容
        //    string[] aryLine = null;
        //    string[] tableHead = null;


        //    ////标示列数
        //    //int columnCount = 0;

        //    //标示是否是读取的第一行
        //    bool IsFirst = true;
        //    //逐行读取CSV中的数据

        //    //_DB.Database.ExecuteSqlRaw("TRUNCATE TABLE sh_adf0979_realtime_tempdata_acc");
        //    List<T> list = new List<T>();
        //    //用一个泛型缓存来存储所有属性，提高性能
        //    PropertyInfo[] props = ReadTimedomainCash<T>.GetProps();

        //    CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;

        //    while ((strLine = sr.ReadLine()) != null)
        //    {

        //        if (IsFirst == true)
        //        {
        //            tableHead = strLine.Split(',');
        //            IsFirst = false;
        //            //把csv里的列名里所有的下划线和空格去除，与我的类名保持一致
        //            for (int i = 0; i < tableHead.Length; i++)
        //            {
        //                var t = tableHead[i].Replace("_", "");
        //                tableHead[i] = t.Replace(" ", "");
        //            }
                    
        //        }
        //        else
        //        {

        //            aryLine = strLine.Split(',');


        //            T titem = new T();


        //            //只要列名与类里的属性名一致就能提取出来，所以类的属性尽量全，只要包含csv里的列名，就可以读取
        //            foreach (var p in props)
        //            {
        //                if (tableHead.ToList().IndexOf(p.Name) != -1)
        //                {
        //                    p.SetValue(titem, Convert.ToDouble(aryLine[tableHead.ToList().IndexOf(p.Name)]));
        //                }

        //            }

        //            list.Add(titem);

        //        }


        //    }
        //    sr.Close();
        //    sr.Dispose();
        //    fs.Close();
        //    fs.Dispose();
        //    //用一个泛型缓存来存储T是否是ACC父类的子类，如果是的话就需要返回父类的speed和brake，提高性能
        //    if (ReadTimedomainCash<T>.ACCorWFT())
        //    {
        //        //把list<T>转为List<TempdataAccBase>父类，父类有速度，GPS和刹车
        //        var ACCList = list.Cast<TempdataAccBase>();
        //        var somedatalist = ACCList.Where(a => a.Time == 0 || a.Time == 1 || a.Time == 2
        //            || a.Time == 3 || a.Time == 4 || a.Time == 5 || a.Time == 6 || a.Time == 7 || a.Time == 8 || a.Time == ACCList.LastOrDefault().Time).Select(b => new
        //            {
                        
        //                b.Speed,
        //                b.Lat,
        //                b.Lon,
        //                b.Spd,
        //                b.Brake
                       
        //            }).ToList();
        //        //判断CSV里有哪个速度，有哪个就用哪个，如果都有或者都没有就用Spd
        //        if (tableHead.Contains("Speed") && !tableHead.Contains("Spd"))
        //        {
        //            speed = somedatalist.Select(a => a.Speed).ToList();
        //        }
        //        else 
        //        {
        //            speed = somedatalist.Select(a => a.Spd).ToList();
        //        }

        //        Brake = somedatalist.Select(a => a.Brake).ToList();
        //        Lat = somedatalist.Select(a => a.Lat).ToList();
        //        Lon = somedatalist.Select(a => a.Lon).ToList();
        //    }
        //    else
        //    {
        //        speed = null;
        //        Brake = null;
        //        Lat = null;
        //        Lon = null;
        //    }
        //    return list;


        //}

        //public List<SatictisData> ReadOneCsvFileForStatistic(string filefullpath, string filename,out string name, out double sdistance)
        //{
        //    try
        //    {
        //        Encoding encoding = Encoding.Default;

        //        name = filename.Split('.')[0];

        //        using FileStream fs = new FileStream(filefullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);

        //        StreamReader sr = new StreamReader(fs, encoding);
        //        DataTable dt = new DataTable();
        //        string strLine = "";
        //        //记录每行记录中的各字段内容
        //        string[] aryLine = null;
        //        string[] tableHead = null;


        //        //标示列数
        //        int columnCount = 0;
        //        //标示是否是读取的第一行
        //        bool IsFirst = true;
        //        //逐行读取CSV中的数,这里要防止读到空格
        //        while ((strLine = sr.ReadLine()) != null)
        //        {

        //            if (IsFirst == true)
        //            {
        //                tableHead = strLine.Split(',');
        //                IsFirst = false;
        //                columnCount = tableHead.Length;
        //                //创建列
        //                for (int i = 0; i < columnCount; i++)
        //                {
        //                    var t = tableHead[i].Replace("_", "");
        //                    tableHead[i] = t.Replace(" ", "");
        //                }

        //                for (int i = 0; i < columnCount; i++)
        //                {
        //                    //把重复的列名改为唯一的名称，否则会出错
        //                    if (tableHead[i].Contains("blank")){
        //                        tableHead[i] = tableHead[i] + i;
        //                    }
        //                    DataColumn dc = new DataColumn(tableHead[i]);

                            
        //                    dt.Columns.Add(dc);
                            
        //                }
        //            }
        //            else
        //            {
        //                aryLine = strLine.Split(',');
        //                if (aryLine[0] != "")//防止读到所有的空行
        //                {
        //                    DataRow dr = dt.NewRow();
        //                    for (int j = 0; j < columnCount; j++)
        //                    {
        //                        dr[j] = aryLine[j];
        //                    }
        //                    dt.Rows.Add(dr);
        //                }

        //            }
        //        }

        //        sr.Close();
        //        sr.Dispose();
        //        fs.Close();
        //        fs.Dispose();
        //        if (aryLine != null && aryLine.Length > 0)
        //        {

        //            List<SatictisData> List = new List<SatictisData>();
        //            //_DB.Database.ExecuteSqlRaw("TRUNCATE TABLE sh_adf0979_satictis_tempdata_acc");
        //            for (int l = 0; l < columnCount - 1; l++)
        //            {

        //                SatictisData entity = new SatictisData();

        //                //entity.Id = name + "-" + l.ToString();
        //                //entity.Time = name;
        //                entity.Chantitle = tableHead[l + 1];

        //                //注意这里由于计算的列的格式是string类型，用max或min计算会出问题，所以必须先转换再求maxmin
        //                //entity.max = dt.Columns[0].Table.AsEnumerable().Select(cols => cols.Field<double>(dt.Columns[0].ColumnName)).Max();
        //                entity.Max = dt.AsEnumerable().Max(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));
        //                entity.Min = dt.AsEnumerable().Min(s => Convert.ToDouble(s.Field<string>(tableHead[l + 1])));

        //                List<string> lst = (from d in dt.AsEnumerable() select d.Field<string>(tableHead[l + 1])).ToList();
        //                double t = 0;
        //                int n = lst.Count;
        //                foreach (var data in lst)
        //                {

        //                    t += Convert.ToDouble(data) * Convert.ToDouble(data);
        //                }


        //                entity.Rms = System.Math.Sqrt(t / n);
        //                //    entity.min = dt.AsEnumerable().Select(t => t.Field<double>(tableHead[l + 1])).Min();
        //                entity.Range = entity.Max - entity.Min;
        //                List.Add(entity);

        //            }
        //            if (tableHead.Contains("Spd"))
        //            {
        //                var speed = List.Where(a => a.Chantitle == "Spd").Select(a => a.Rms).ToList().FirstOrDefault();//选用speed的rms来作为计算里程的参数
        //                sdistance = speed * 10 / 3600;

        //            }
        //            else
        //            {
        //                sdistance = 0;
        //            }
        //            return List;
        //        }
        //        else
        //        {
        //            sdistance = 0;
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
            


        //}

       
        /// <summary>
        /// 用一个方法读取csv文件里的时域数据及统计数据
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public CsvFileReturnAllList<T> ReadCSVFileAll(string filefullpath, string filename)
        {
            return CSVFileOperation<T>.ReadCSVFileAll(filefullpath, filename);
        }
    }
}
