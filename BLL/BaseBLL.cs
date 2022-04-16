using IDAL;
using Microsoft.EntityFrameworkCore;
using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.FileOperation;

namespace BLL
{
    public abstract class BaseBLL<T> where T : class, new()
    {
        
        public IBaseDAL<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseBLL()
        {
                       
            SetCurrentDal();//子类一定要实现抽象方法，一旦创建抽象子类的实例时，就会调用此方法
        }

        public IQueryable<T> LoadEntities
         (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }

        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return CurrentDal.SaveChanges();
        }
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return CurrentDal.SaveChanges();
        }
        public bool AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);

            return CurrentDal.SaveChanges();
        }
        //public List<T> ReadOneCsvFileService(string filefullpath, string filename, out string name, out List<double> speed, out List<double> Brake, out List<double> Lat, out List<double> Lon)
        //{
        //    return CurrentDal.ReadOneCsvFile(filefullpath, filename,out name,out speed,out Brake, out Lat,out Lon);

        //}
        //public List<SatictisData> ReadOneCsvFileForStatisticService(string filefullpath, string filename, out string name, out double sdistance)
        //{
        //    return CurrentDal.ReadOneCsvFileForStatistic(filefullpath, filename, out name, out sdistance);

        //}
       

        public async Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename)
        {
            return await CurrentDal.ReadCSVFileAll(filefullpath, filename);
        }

    }
}
