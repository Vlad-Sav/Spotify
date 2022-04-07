using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Data.Repos
{
    public interface IRepo<T>
    {
        int Add(T entity);
        int AddRange(List<T> entities);
        int Save(T entity);
        int Delete(int id);
        int Delete(T entity);
        T GetOne(int? id);
        List<T> GetAll();
        List<T> ExecuteQuery(string sql);
        List<T> ExecuteQuery(string sql, object[] sqlParametersObjects);
    }
}
