using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;
using System.Web;
using InsightWebApi.Models;

namespace InsightWebApi.DAL
{
    public class Repositories: IRepositories, IDisposable
    {
        /// <summary>
        /// Declare object of DbContext class
        /// </summary>
        public readonly ApplicationDbContext context;

        //private readonly string _connString = ConfigurationManager.ConnectionStrings["FacelessContext"].ConnectionString;

        /// <summary>
        /// Used to intialize object of Repository class.
        /// </summary>
        public Repositories()
        {
            context = context ?? (context = new ApplicationDbContext());
        }

        #region IRepository Members

        /// <summary>
        /// This method is used to Find a record in entity by its id of long type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById<TEntity>(object id) where TEntity : class
        {
            try
            {
                return context.Set<TEntity>().Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Find a record in entity by its id of string type.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById<TEntity>(string id) where TEntity : class
        {
            try
            {
                return context.Set<TEntity>().Find(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add aa new record in database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                context.Set<TEntity>().Add(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Add aa new record in database.
        /// </summary>
        /// <param name="entity"></param>
        /// 
        public virtual void AddOrUpdate<TEntity>(Expression<Func<TEntity, object>> identifier, params TEntity[] entities) where TEntity : class
        {
            try
            {
                context.Set<TEntity>().AddOrUpdate(identifier, entities);

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Update a record in database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                context.Set<TEntity>();
                context.Entry<TEntity>(entity).State = EntityState.Modified;

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Delete a record in database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                context.Set<TEntity>().Attach(entity);
                context.Set<TEntity>().Remove(entity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void Delete<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            var objects = GetMany<TEntity>(whereClause);

            foreach (var obj in objects)
                Delete(obj);

        }

        /// <summary>
        /// This method is used Get a particular record according to LINQ predicate.
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public virtual TEntity Get<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            try
            {
                return context.Set<TEntity>().Where<TEntity>(whereClause.Compile()).FirstOrDefault<TEntity>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual IEnumerable<TEntity> GetWithoutProxy<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            try
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    context.Configuration.ProxyCreationEnabled = false;
                    return context.Set<TEntity>().Where<TEntity>(whereClause.Compile()).ToList<TEntity>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Get all the records present is an entity.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            try
            {
                return context.Set<TEntity>().ToList<TEntity>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAllReadOnly<TEntity>() where TEntity : class
        {
            try
            {
                return context.Set<TEntity>().AsNoTracking().ToList<TEntity>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used Get a list of records according to LINQ predicate.
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetMany<TEntity>(Expression<Func<TEntity, bool>> whereClause) where TEntity : class
        {
            try
            {
                return context.Set<TEntity>().Where(whereClause.Compile()).ToList();
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Used to Execute a Stored Procedure for select statements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="commandName"></param>
        /// <param name="parameteres"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> RunStoredProcedure<TEntity>(string commandName, params object[] parameteres) where TEntity : class
        {
            try
            {
                return context.Database.SqlQuery<TEntity>(commandName, parameteres);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// This method is used for insert/update/delete
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="parameteres"></param>
        public int RunStoredProcedure(string commandName, params object[] parameteres)
        {
            try
            {
                return context.Database.ExecuteSqlCommand(commandName, parameteres);
                //return Convert.ToInt32(context.Database.SqlQuery<TEntity>(commandName, parameteres).FirstOrDefault());
            }
            catch (Exception)
            {

                throw;
            }
        }


        public virtual IEnumerable<DbEntityValidationResult> Validate()
        {
            try
            {
                return context.GetValidationErrors();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method is used to Make Save changes in DB.
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion



        /// <summary>
        /// This method is used to Update a record in database.
        /// </summary>
        /// <param name="entity"></param>
        public virtual DbSet<TEntity> GetGet<TEntity>() where TEntity : class
        {
            try
            {
                return context.Set<TEntity>();
                //context.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region IDisposable Members

        /// <summary>
        /// Declare variable to indicate disposed state of object
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// This method is used to Dispose the context
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// This method is used to call Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}