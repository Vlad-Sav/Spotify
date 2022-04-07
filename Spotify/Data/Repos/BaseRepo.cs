using Microsoft.EntityFrameworkCore;
using Spotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotify.Data.Repos
{
    public class BaseRepo<T> : IDisposable, IRepo<T> where T : BaseEntity, new()
    {
        protected readonly SpotifyFuncContext _db;
        protected readonly DbSet<T> _table;

        public BaseRepo()
        {
            _db = new SpotifyFuncContext();
            _table = _db.Set<T>();
        }
        protected SpotifyFuncContext Context => _db;
        public void Dispose()
        {
            _db?.Dispose();
        }
        public T GetOne(int? id) => _table.Find(id);
        public virtual List<T> GetAll() => _table.ToList();
        public int Add(T entity)
        {
            _table.Add(entity);
            return SaveChanges();
        }

        public int AddRange(List<T> entities)
        {
            _table.AddRange(entities);
            return SaveChanges();
        }

        public int Save(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            return SaveChanges();
        }

        public virtual int Delete(int id)
        {
          
            _db.Entry(new T { Id = id}).State = EntityState.Deleted;
            
            return SaveChanges();
        }

        public int Delete(T entity)
        {
            _db.Entry(entity).State = EntityState.Deleted;
            return SaveChanges();
            
        }

        public List<T> ExecuteQuery(string sql) => _table.FromSqlRaw(sql).ToList();

        public List<T> ExecuteQuery(string sql, object[] sqlParametersObjects) => _table.FromSqlRaw(sql, sqlParametersObjects).ToList();
        internal int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                
                throw;
            }
            catch (DbUpdateException ex)
            {
               
                throw;
            }
           
            catch (Exception ex)
            {
                
                throw;
            }
        }


    }
}
