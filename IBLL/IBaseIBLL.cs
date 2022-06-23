using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.FileOperation;

namespace IBLL
{
    public interface IBaseIBLL<T> where T : class, new()
    {

        IQueryable<T> LoadEntities
         (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        bool DeleteEntity(T entity);
        bool DeleteAllEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        bool EditEntity(T entity);
        bool AddEntity(T entity);
        //List<T> ReadOneCsvFileService(string filefullpath, string filename, out string name, out List<double> speed, out List<double> Brake, out List<double> Lat, out List<double> Lon);
        //List<SatictisData> ReadOneCsvFileForStatisticService(string filefullpath, string filename, out string name, out double sdistance);
        
      
        Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename);

    }
}
