using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Common.Core;

namespace Data.Repository
{
    public interface IRepository<T>
    {
        Task<bool> Insert(T entity, Ref<CheckError> checkError = null);
        Task<bool> InsertRange(IList<T> list, Ref<CheckError> checkError = null);
        Task<bool> Edit(T entity, IList<string> propertyNamesNotChanged = null, Ref<CheckError> checkError = null);
        Task<bool> Edit(IList<T> entitys, IList<string> propertyNamesNotChanged = null, Ref<CheckError> checkError = null);
        Task<bool> Delete(T entity, Ref<CheckError> checkError = null);
        Task<bool> Delete(object id, Ref<CheckError> checkError = null);
        Task<bool> DeleteAll(IList<T> list, Ref<CheckError> checkError = null);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);
        Task<bool> CheckExist(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, int skip, int take, Ref<CheckError> checkError = null);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string groupBy, string fieldOrderBy, bool ascending, int take, Ref<CheckError> checkError = null);
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string fieldOrderBy, bool ascending, Ref<CheckError> checkError = null);
        Task<IEnumerable<T>> GetAll(Ref<CheckError> checkError = null);
        Task<int> GetCount(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);
        Task<T> GetById(object id, Ref<CheckError> checkError = null);
        Task<T> GetOne(Expression<Func<T, bool>> predicate, Ref<CheckError> checkError = null);
        Task<IEnumerable<T>> Get(string query, SqlParameter[] parameters = null, Ref<CheckError> checkError = null);
        Task<T> GetOne(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null);
        Task<IEnumerable<SqlParameter>> GetOutPut(string storedProcedureName, SqlParameter[] parameters = null, Ref<CheckError> checkError = null);
    }
}
