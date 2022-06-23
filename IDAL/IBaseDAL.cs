using MysqlforDataWatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.FileOperation;

namespace IDAL
{
    public interface IBaseDAL<T> where T : class, new()
    {
        IQueryable<T> LoadEntities
          (System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        bool DeleteEntity(T entity);

        bool DeleteAllEntity(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        bool EditEntity(T entity);
        bool AddEntity(T entity);
        bool SaveChanges();
              
        Task<CsvFileReturnAllList<T>> ReadCSVFileAll(string filefullpath, string filename);

    }
}
