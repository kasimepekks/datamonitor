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
using System.Threading.Tasks;
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
        /// <summary>
        /// 删除所选的数据
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public bool DeleteAllEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            var list=_DB.Set<T>().Where<T>(whereLambda);
            if (list.Count() > 0)
            {
                _DB.Set<T>().RemoveRange(list);
                return true;
            }
            else
            {
                return false;
            }
            
            
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
        /// 用一个方法读取csv文件里的时域数据及统计数据进行展示，不要分开用2个方法了
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename)
        {
            return await CSVFileOperation<T>.ReadCSVFileAll(filefullpath, filename);
        }
    }
}
