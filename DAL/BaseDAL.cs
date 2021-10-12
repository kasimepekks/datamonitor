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
        /// <param name="brake"></param>
        /// <returns></returns>
       public List<T> ReadOneCsvFile(string filefullpath, string filename, out string name,out List<double> speed, out List<double> brake, out List<double> Lat, out List<double> Lon)
        {
            
            Encoding encoding = Encoding.Default;

             name = filename.Split('.')[0];
            //string starttime = name.Split('-')[1];
            FileStream fs = new FileStream(filefullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, encoding);

            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;


            ////标示列数
            //int columnCount = 0;

            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据

            //_DB.Database.ExecuteSqlRaw("TRUNCATE TABLE sh_adf0979_realtime_tempdata_acc");
            List<T> list = new List<T>();
           //用一个泛型缓存来存储所有属性，提高性能
            PropertyInfo[] props = ReadTimedomainCash<T>.GetProps();

            CompareInfo Compare = CultureInfo.InvariantCulture.CompareInfo;

            while ((strLine = sr.ReadLine()) != null)
            {

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    for (int i = 0; i < tableHead.Length; i++)
                    {
                        var t= tableHead[i].Replace("_", "");
                        tableHead[i] = t.Replace(" ", "");
                    }
                    //foreach (var item in tableHead)
                    //{
                    //    item.Replace("_", "");
                    //    item.Trim();
                    //}
                }
                else
                {

                    aryLine = strLine.Split(',');

              
                    T titem = new T();

                                       
                        //只要列名与类里的属性名一致就能提取出来
                     foreach (var p in props)
                     {
                        if (tableHead.ToList().IndexOf(p.Name) != -1)
                        {
                            p.SetValue(titem, Convert.ToDouble(aryLine[tableHead.ToList().IndexOf(p.Name)]));
                        }
                                                   
                     }
                        
                    list.Add(titem);

                }


            }
            sr.Close();
            sr.Dispose();
            fs.Close();
            fs.Dispose();
            //用一个泛型缓存来存储T是否是ACC父类的子类，如果是的话就需要返回父类的speed和brake，提高性能
            if (ReadTimedomainCash<T>.ACCorWFT())
            {
                //把list<T>转为List<TempdataAccBase>父类，父类有速度，GPS和刹车
                var ACCList =list.Cast<TempdataAccBase>();
                var somedatalist = ACCList.Where(a => a.Time == 0 || a.Time == 1 || a.Time == 2
                    || a.Time == 3 || a.Time == 4 || a.Time == 5 || a.Time == 6 || a.Time == 7 || a.Time == 8 || a.Time == 9).Select(b => new {
                      b.Brake,
                      b.Speed,
                      b.Lat,
                      b.Lon
                   }).ToList();
                speed = somedatalist.Select(a => a.Speed).ToList();
                brake = somedatalist.Select(a => a.Brake).ToList();
                Lat = somedatalist.Select(a => a.Lat).ToList();
                Lon = somedatalist.Select(a => a.Lon).ToList();
            }
            else
            {
                speed = null;
                brake = null;
                Lat = null;
                Lon = null;
            }
            return list;
            
        }

       public List<SatictisData> ReadOneCsvFileForStatistic(string filefullpath, string filename,out string name, out double sdistance)
        {
            Encoding encoding = Encoding.Default;

             name = filename.Split('.')[0];

            FileStream fs = new FileStream(filefullpath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
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
            //逐行读取CSV中的数,这里要防止读到空格
            while ((strLine = sr.ReadLine()) != null)
            {

                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
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
                    if (aryLine[0] != "")//防止读到所有的空行
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                   
                }
            }

            sr.Close();
            sr.Dispose();
            fs.Close();
            fs.Dispose();
            if (aryLine != null && aryLine.Length > 0)
            {
                //先删除表中所有数据再添加最新的数据并在前端显示，这样前端就能实时显示了
                List<SatictisData> List = new List<SatictisData>();
                //_DB.Database.ExecuteSqlRaw("TRUNCATE TABLE sh_adf0979_satictis_tempdata_acc");
                for (int l = 0; l < columnCount - 1; l++)
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
                    List.Add(entity);

                }
                if (tableHead.Contains("Speed")){
                    var speed = List.Where(a => a.Chantitle == "Speed").Select(a => a.Rms).ToList().FirstOrDefault();//选用speed的rms来作为计算里程的参数
                    sdistance = speed * 10 / 3600;
                   
                }
                else
                {
                    sdistance = 0;
                }
                return List;
            }
            else
            {
                sdistance = 0;
                return null;
            }

        }

      

    }
}
