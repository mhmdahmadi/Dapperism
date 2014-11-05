using System.Collections.Generic;
using System.Data;
using Dapperism.Entity;
using Dapperism.Enums;
using Dapperism.Query;
using Dapperism.Validation;

namespace Dapperism.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        ValidationResults ValidationResults { get; set; }
        IDbTransaction BeginTransaction();
        void CommitTransaction(IDbTransaction transaction);
        void RollbackTransaction(IDbTransaction transaction);
        TEntity Insert(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null);
        void Insert(IList<TEntity> entities, MethodType method = MethodType.Text, IDbTransaction transaction = null);
        void Update(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null);
        void Update(IList<TEntity> entities, MethodType method = MethodType.Text, IDbTransaction transaction = null);
        TEntity InsertOrUpdate(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null);
        void Delete(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null);
        void Delete(IList<TEntity> entities, MethodType method = MethodType.Text, IDbTransaction transaction = null);
        void Delete(MethodType method = MethodType.Text, IDbTransaction transaction = null, params object[] id);

        IEnumerable<TEntity> GetAll(int? pageIndex = null, int? pageSize = null, string[] pagingOrderCols = null,
            bool? isAscendingOrder = null, IDbTransaction transaction = null, params string[] selectClause);

        IEnumerable<TEntity> GetAllBySp(IDbTransaction transaction = null);
        TEntity GetById(MethodType method = MethodType.Text, IDbTransaction transaction = null, string[] selectClause = null, params object[] id);

        IEnumerable<object> ExecSqlTableFunction(string functionName, IDbTransaction transaction = null,
            string[] selectClause = null, params string[] argumens);

        dynamic ExecSqlScalarFunction(string functionName, IDbTransaction transaction = null,
            params string[] argumens);

        IEnumerable<TEntity> ExecStoredProcedure(string spName, object param = null,
            IDbTransaction transaction = null);

        IEnumerable<TEntity> ExecStoredProcedure(string spName,
            DynamicParameters dynamicParams, IDbTransaction transaction = null);

        IEnumerable<object> ExecDynamicStoredProcedure(string spName, object param = null,
            IDbTransaction transaction = null);

        IEnumerable<object> ExecDynamicStoredProcedure(string spName,
            DynamicParameters dynamicParams, IDbTransaction transaction = null);

        IEnumerable<TEntity> GetByFilter(QueryExpression<TEntity> query, IDbTransaction transaction = null);
    }
}