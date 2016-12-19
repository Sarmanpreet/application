using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace InsightWebApi.DAL
{
    public interface IRepositories
    {
        TEntity GetById<TEntity>(object id) where TEntity : class;
        TEntity GetById<TEntity>(string id) where TEntity : class;
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void AddOrUpdate<TEntity>(Expression<Func<TEntity, object>> identifier, params TEntity[] entities) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;
        TEntity Get<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;
        IEnumerable<TEntity> GetWithoutProxy<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        IEnumerable<TEntity> GetAllReadOnly<TEntity>() where TEntity : class;
        IEnumerable<TEntity> GetMany<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class;
        IEnumerable<TEntity> RunStoredProcedure<TEntity>(string commandName, params object[] parameteres) where TEntity : class;
        int RunStoredProcedure(string commandName, params object[] parameteres);
        DbSet<TEntity> GetGet<TEntity>() where TEntity : class;
        IEnumerable<DbEntityValidationResult> Validate();
        void SaveChanges();
        void Dispose();
    }
}