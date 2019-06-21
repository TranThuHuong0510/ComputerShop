using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Reflection;
using System.Data;
using Common.Core;
using Data;

namespace Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        readonly DatabaseContext _dataContext;
        public Repository()
        {
            _dataContext = new DatabaseContext();
        }
        #region IRepository<T> Members

        /// <summary>
        /// Insert entity in DataBase
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> Insert(T entity, Ref<CheckError> checkError = null)
        {
            try
            {
                _dataContext.Set<T>().Add(entity);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Insert(T entity, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }
        /// <summary>
        /// Insert multiple rows to database
        /// </summary>
        /// <param name="list"></param>
        /// <param name="checkError">Check Error</param>
        /// <returns></returns>
        public async Task<bool> InsertRange(IList<T> list, Ref<CheckError> checkError = null)
        {
            try
            {
                _dataContext.Set<T>().AddRange(list);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "InsertRange(IList<T> list, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }
        /// <summary>
        /// Update entity in DataBase
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="propertyNamesNotChanged">Property names not change</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> Edit(T entity, IList<string> propertyNamesNotChanged = null, Ref<CheckError> checkError = null)
        {
            try
            {
                _dataContext.Set<T>().Attach(entity);
                _dataContext.Entry(entity).State = EntityState.Modified;
                if (propertyNamesNotChanged != null)
                {
                    var ignoredProperties = DatabaseContext.IgnoredProperties[typeof(T)];

                    var propertyNames = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => (ignoredProperties == null || !ignoredProperties.Contains(x)));

                    var propertyNotChangedNames = from x in propertyNames
                                                  where propertyNamesNotChanged.Contains(x.Name)
                                                  select x.Name;
                    foreach (var propertyName in propertyNotChangedNames)
                    {
                        _dataContext.Entry(entity).Property(propertyName).IsModified = false;
                    }
                }

                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Edit(T entity, IList<string> propertyNamesNotChanged = null, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }
        /// <summary>
        /// Update entity in DataBase
        /// </summary>
        /// <param name="entitys"> List entity Update</param>
        /// <param name="propertyNamesNotChanged">Property names not change</param>
        /// <param name="checkError">Check Error</param>
        /// <returns></returns>
        public async Task<bool> Edit(IList<T> entitys, IList<string> propertyNamesNotChanged = null, Ref<CheckError> checkError = null)
        {
            try
            {
                foreach (var entity in entitys)
                {
                    _dataContext.Set<T>().Attach(entity);
                    _dataContext.Entry(entity).State = EntityState.Modified;
                    if (propertyNamesNotChanged != null)
                    {
                        var ignoredProperties = DatabaseContext.IgnoredProperties[typeof(T)];

                        var propertyNames = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => (ignoredProperties == null || !ignoredProperties.Contains(x)));

                        var propertyNotChangedNames = from x in propertyNames
                                                      where propertyNamesNotChanged.Contains(x.Name)
                                                      select x.Name;
                        foreach (var propertyName in propertyNotChangedNames)
                        {
                            _dataContext.Entry(entity).Property(propertyName).IsModified = false;
                        }
                    }

                }
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Edit(IList<T> entitys, IList<string> propertyNamesNotChanged = null, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }
        /// <summary>
        /// Delete entity in DataBase
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> Delete(T entity, Ref<CheckError> checkError = null)
        {
            try
            {
                _dataContext.Set<T>().Remove(entity);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Delete(T entity, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }

        /// <summary>
        /// Delete model in DataBase
        /// </summary>
        /// <param name="id">Key in entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> Delete(object id, Ref<CheckError> checkError = null)
        {
            try
            {
                T entity = await GetById(id);
                if (entity != null)
                {
                    _dataContext.Set<T>().Remove(entity);
                    return await _dataContext.SaveChangesAsync() > 0;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Delete(object id, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }

        /// <summary>
        /// Delete multiple records
        /// </summary>
        /// <param name="list">list entity</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>Boolean</returns>
        public async Task<bool> DeleteAll(IList<T> list, Ref<CheckError> checkError = null)
        {
            try
            {
                _dataContext.Set<T>().RemoveRange(list);
                return await _dataContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "DeleteAll(IList<T> list, Ref<CheckError> checkError = null)", ex);
                return false;
            }
        }

        /// <summary>
        /// Get list entity by condition
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>list entity</returns>
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<T>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Check Exist
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="checkError">Check Error</param>
        /// <returns></returns>
        public async Task<bool> CheckExist(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<T>().AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "CheckExist(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
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
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(T).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, int>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending
                        ? _dataContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync()
                        : _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                }
                else
                {
                    if (t == typeof(int?))
                    {
                        var pe = Expression.Parameter(typeof(T), "p");
                        var expr1 = Expression.Lambda<Func<T, int?>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending
                            ? _dataContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync()
                            : _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }
                    else if (t == typeof(DateTime))
                    {
                        var pe = Expression.Parameter(typeof(T), "p");
                        var expr1 = Expression.Lambda<Func<T, DateTime>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending ? _dataContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync() : _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }
                    else
                    {
                        var pe = Expression.Parameter(typeof(T), "p");
                        var expr1 = Expression.Lambda<Func<T, String>>(Expression.Property(pe, fieldOrderBy), pe);
                        return await (ascending
                            ? _dataContext.Set<T>().Where(predicate).OrderBy(expr1).ToListAsync()
                            : _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).ToListAsync());
                    }

                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, Ref<CheckError> checkError = null)", ex);
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
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, int skip, int take, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(T).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, int>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dataContext.Set<T>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
                else if (t == typeof(DateTime))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, DateTime>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dataContext.Set<T>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
                else
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, string>>(Expression.Property(pe, fieldOrderBy), pe);
                    return await (ascending ? _dataContext.Set<T>().Where(predicate).OrderBy(expr1).Skip(skip).Take(take).ToListAsync() : _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).Skip(skip).Take(take).ToListAsync());
                }
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, int skip, int take, Ref<CheckError> checkError = null)", ex);
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
        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string groupBy, string fieldOrderBy, bool ascending, int take, Ref<CheckError> checkError = null)
        {
            try
            {
                var p = typeof(T).GetProperty(fieldOrderBy);
                var t = p.PropertyType;
                if (t == typeof(int))
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, int>>(Expression.Property(pe, fieldOrderBy), pe);

                    var fieldXExpression = Expression.Property(pe, groupBy);
                    var lambda = Expression.Lambda<Func<T, object>>(
                        fieldXExpression,
                        pe);
                    if (ascending)
                    {
                        var data = await _dataContext.Set<T>().Where(predicate).OrderBy(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                    else
                    {
                        var data = await _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                }
                else
                {
                    var pe = Expression.Parameter(typeof(T), "p");
                    var expr1 = Expression.Lambda<Func<T, string>>(Expression.Property(pe, fieldOrderBy), pe);
                    var fieldXExpression = Expression.Property(pe, groupBy);
                    var lambda = Expression.Lambda<Func<T, object>>(
                       fieldXExpression,
                        pe);
                    if (ascending)
                    {
                        var data = await _dataContext.Set<T>().Where(predicate).OrderBy(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
                        foreach (var item in data)
                        {
                            list.AddRange(item);
                        }
                        return list;
                    }
                    else
                    {
                        var data = await _dataContext.Set<T>().Where(predicate).OrderByDescending(expr1).GroupBy(lambda).Select(x => x.ToList().Take(take)).ToListAsync();
                        var list = new List<T>();
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

                Logger.CreateLog(Logger.Levels.ERROR, this, "Get(Expression<Func<T, bool>> predicate, string groupBy, string fieldOrderBy, bool ascending, int take, Ref<CheckError> checkError = null)", ex);
                return null;
            }
        }

        /// <summary>
        /// Get count list entity by condition
        /// </summary>
        /// <param name="predicate">condition</param>
        /// <param name="checkError">Check Error</param>
        /// <returns>count list entity by condition </returns>
        public async Task<int> GetCount(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<T>().Where(predicate).CountAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetCount(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
                return -1;
            }
        }

        /// <summary>
        /// Get all entity
        /// </summary>
        /// <param name="checkError">Check Error</param>
        /// <returns>list entity</returns>
        public async Task<IEnumerable<T>> GetAll(Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<T>().ToListAsync();
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
        public async Task<T> GetById(object id, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<T>().FindAsync(id);
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
        public async Task<T> GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)
        {
            try
            {
                return await _dataContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                if (checkError != null)
                {
                    checkError.Value = new CheckError() { IsError = true, Exception = ex, Message = ex.Message };
                }

                Logger.CreateLog(Logger.Levels.ERROR, this, "GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null)", ex);
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
        public async Task<IEnumerable<T>> Get(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
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

                    return await _dataContext.Database.SqlQuery<T>(query, parameters).ToListAsync();
                }
                else
                {
                    return await _dataContext.Database.SqlQuery<T>(storedProcedureName).ToListAsync();
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
        public async Task<T> GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null)
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

                    return await _dataContext.Database.SqlQuery<T>(query, parameters).FirstOrDefaultAsync();
                }
                else
                {
                    return await _dataContext.Database.SqlQuery<T>(storedProcedureName).FirstOrDefaultAsync();
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

        public IQueryable<T> AsQueryable()
        {
            return _dataContext.Set<T>().AsQueryable<T>();
        }
    }
}
