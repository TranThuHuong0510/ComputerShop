using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Core;
using Data;

namespace Data.Repository
{
    /// <summary>
    /// Generic Repository class for Entity Operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> where TEntity : class
    {
        readonly DatabaseContext _dataContext;
        readonly DbSet<TEntity> _dbSet;

        #region Public Constructor...
        /// <summary>
        /// Public Constructor,initializes privately declared local variables.
        /// </summary>
        /// <param name="_dataContext"></param>
        public GenericRepository(DatabaseContext _dataContext)
        {
            this._dataContext = _dataContext;
            this._dbSet = _dataContext.Set<TEntity>();
        }
        #endregion

        #region Public methods

        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        /// <summary>
        /// generic Insert method for the entities
        /// </summary>
        /// <param name="lstEntity"></param>
        public virtual void Insert(IList<TEntity> lstEntity)
        {
            _dbSet.AddRange(lstEntity);
        }

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _dataContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Generic update method for the entities
        /// </summary>
        /// <param name="lstEntityToUpdate"></param>
        /// <param name="propertyNamesNotChanged"></param>
        public virtual void Update(IList<TEntity> lstEntityToUpdate, IList<string> propertyNamesNotChanged = null)
        {
            foreach (var entityToUpdate in lstEntityToUpdate)
            {
                _dbSet.Attach(entityToUpdate);
                _dataContext.Entry(lstEntityToUpdate).State = EntityState.Modified;

                if (propertyNamesNotChanged != null)
                {
                    var ignoredProperties = DatabaseContext.IgnoredProperties[typeof(TEntity)];

                    var propertyNames = entityToUpdate.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => (ignoredProperties == null || !ignoredProperties.Contains(x)));

                    var propertyNotChangedNames = from x in propertyNames
                                                  where propertyNamesNotChanged.Contains(x.Name)
                                                  select x.Name;
                    foreach (var propertyName in propertyNotChangedNames)
                    {
                        _dataContext.Entry(entityToUpdate).Property(propertyName).IsModified = false;
                    }
                }
            }
        }

        /// <summary>
        /// Generic delte method for the entities
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            _dataContext.Set<TEntity>().Remove(entity);
        }
        /// <summary>
        /// Generic delete all method for the list entities
        /// </summary>
        /// <param name="list"></param>
        public virtual void DeleteAll(IList<TEntity> list)
        {
            _dataContext.Set<TEntity>().RemoveRange(list);
        }

        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Check Exist
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="checkError">Check Error</param>
        /// <returns></returns>
        public async Task<bool> CheckExist(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<TEntity>().AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "CheckExist(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }
        /// <summary>
        /// Get list entity by condition
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="fieldOrderBy">field order by</param>
        /// <param name="ascending">sort</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>list entity</returns>
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, string fieldOrderBy, bool ascending, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(TEntity).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(TEntity), "p");
                    var expr1 = Expression.Lambda<Func<TEntity, int>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending
                        ? _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).ToListAsync()
                        : _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                }
                else
                {
                    if (t == typeof(int?))
                    {
                        var pe = Expression.Parameter(typeof(TEntity), "p");
                        var expr1 = Expression.Lambda<Func<TEntity, int?>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending
                            ? _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).ToListAsync()
                            : _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }
                    else if (t == typeof(DateTime))
                    {
                        var pe = Expression.Parameter(typeof(TEntity), "p");
                        var expr1 = Expression.Lambda<Func<TEntity, DateTime>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending ? _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).ToListAsync() : _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }
                    else
                    {
                        var pe = Expression.Parameter(typeof(TEntity), "p");
                        var expr1 = Expression.Lambda<Func<TEntity, String>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending
                            ? _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).ToListAsync()
                            : _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }

                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<TEntity, bool>> predicate, string fieldOrderBy, bool ascending, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get list entity by condition
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="fieldOrderBy">field order by</param>
        /// <param name="ascending">sort</param>
        /// <param name="skip">skip</param>
        /// <param name="take">take</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>list entity</returns>
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, string fieldOrderBy, bool ascending, int skip, int take, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(TEntity).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(TEntity), "p");
                    var expr1 = Expression.Lambda<Func<TEntity, int>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
                else if (t == typeof(DateTime))
                {
                    var pe = Expression.Parameter(typeof(TEntity), "p");
                    var expr1 = Expression.Lambda<Func<TEntity, DateTime>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
                else
                {
                    var pe = Expression.Parameter(typeof(TEntity), "p");
                    var expr1 = Expression.Lambda<Func<TEntity, string>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<TEntity, bool>> predicate, string fieldOrderBy, bool ascending, int skip, int take, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        ///  Get list entity by condition
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="groupBy">group by</param>
        /// <param name="fieldOrderBy">field order by</param>
        /// <param name="ascending">sort</param>
        /// <param name="take">take</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>list entity by condition</returns>
        public async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> predicate, string groupBy, string fieldOrderBy, bool ascending, int take, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(TEntity).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(TEntity), "p");
                    var expr1 = Expression.Lambda<Func<TEntity, int>>(Expression.Property(pe, fieldOrderBy), pe);

                    var fieldXExpression = Expression.Property(pe, groupBy);
                    var lambda = Expression.Lambda<Func<TEntity, object>>(
                        fieldXExpression,
                        pe);
                    if (ascending)
                    {
                        var data = await _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<TEntity>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                    else
                    {
                        var data = await _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<TEntity>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                }
                else
                {
                    var pe = Expression.Parameter(typeof(TEntity), "p");
                    var expr1 = Expression.Lambda<Func<TEntity, string>>(Expression.Property(pe, fieldOrderBy), pe);
                    var fieldXExpression = Expression.Property(pe, groupBy);
                    var lambda = Expression.Lambda<Func<TEntity, object>>(
                       fieldXExpression,
                        pe);
                    if (ascending)
                    {
                        var data = await _dataContext.Set<TEntity>().Where(predicate).OrderBy(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<TEntity>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                    else
                    {
                        var data = await _dataContext.Set<TEntity>().Where(predicate).OrderByDescending(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<TEntity>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<TEntity, bool>> predicate, string groupBy, string fieldOrderBy, bool ascending, int take, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        public async Task<int> GetCount(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<TEntity>().Where(predicate).CountAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetCount(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return -1;
            }
        }

        /// <summary>
        /// Get all entity
        /// </summary>
        /// <param name="checkError">Check Error</param>
        /// <returns>list entity</returns>
        public async Task<IEnumerable<TEntity>> GetAll(Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetAll(Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="id">Key in entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>entity</returns>
        public async Task<TEntity> GetById(object id, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetById(object id, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>entity</returns>
        public async Task<TEntity> GetOne(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<TEntity>().Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(Expression<Func<TEntity, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get list entity
        /// </summary>
        /// <param name="storedProcedureName">stored procedure name</param>
        /// <param name="parameters">parameters input</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>list entity</returns>
        public async Task<IEnumerable<TEntity>> Get(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("Exec ", storedProcedureName, " ");

                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;
                        query += string.Concat(itemObject.ParameterName, ",");
                    }
                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    return await _dataContext.Database.SqlQuery<TEntity>(query, parameters).ToListAsync();
                }
                else
                {
                    return await _dataContext.Database.SqlQuery<TEntity>(storedProcedureName).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get entity
        /// </summary>
        /// <param name="storedProcedureName">stored procedure name</param>
        /// <param name="parameters">parameters[]</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>entity</returns>
        public async Task<TEntity> GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("Exec ", storedProcedureName, " ");

                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;
                        query += string.Concat(itemObject.ParameterName, ",");
                    }
                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    return await _dataContext.Database.SqlQuery<TEntity>(query, parameters).FirstOrDefaultAsync();
                }
                else
                {
                    return await _dataContext.Database.SqlQuery<TEntity>(storedProcedureName).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get Out Put
        /// </summary>
        /// <param name="storedProcedureName">stored procedure name</param>
        /// <param name="parameters">parameters[]</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>List SqlParameter</returns>
        public async Task<IEnumerable<SqlParameter>> GetOutPut(string storedProcedureName, SqlParameter[] parameters, Ref<CheckError> checkError = null)
        {
            try
            {
                if (parameters != null)
                {
                    var query = string.Concat("", storedProcedureName, " ");

                    var listParameterOutPut = new List<SqlParameter>();

                    foreach (var item in parameters)
                    {
                        var itemObject = (SqlParameter)item;

                        if (itemObject.Direction == ParameterDirection.Output)
                        {
                            listParameterOutPut.Add(itemObject);
                            query += string.Concat(itemObject.ParameterName, " OUT,");
                        }
                        else
                        {
                            query += string.Concat(itemObject.ParameterName, ",");
                        }
                    }
                    query = parameters.Length > 0 ? query.Substring(0, query.Length - 1) : storedProcedureName;

                    await _dataContext.Database.ExecuteSqlCommandAsync(query, parameters);

                    return listParameterOutPut;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetOutPut(string storedProcedureName, SqlParameter[] parameters, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        #endregion
    }
}
