using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools.FileOperation;

namespace IDAL
{
    public interface IBaseDAL<T> where T : class, new()
    {
        IQueryable<T> LoadEntities
          (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        bool DeleteEntity(T entity);
        bool EditEntity(T entity);
        bool AddEntity(T entity);
        bool SaveChanges();
        //List<T> ReadOneCsvFile(string filefullpath, string filename, out string name, out List<double> speed, out List<double> Brake, out List<double> Lat, out List<double> Lon);
        //List<SatictisData> ReadOneCsvFileForStatistic(string filefullpath, string filename, out string name, out double sdistance);
              
        CsvFileReturnAllList<T> ReadCSVFileAll(string filefullpath, string filename);

    }
}
