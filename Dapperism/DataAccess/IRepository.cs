using System.Collections.Generic;
using System.Data;
using Dapperism.Entities;
using Dapperism.Enums;

namespace Dapperism.DataAccess
{
    public interface IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        ValidationResults ValidationResults { get; set; }
        IDbTransaction BeginTransaction();
        void CommitTransaction(IDbTransaction transaction);
        void RollbackTransaction(IDbTransaction transaction);
        TEntity Insert(TEntity entity, IDbTransaction transaction = null);
        void Insert(IList<TEntity> entities, IDbTransaction transaction = null);
        TEntity InsertBySp(TEntity entity, IDbTransaction transaction = null);
        void InsertBySp(IList<TEntity> entities, IDbTransaction transaction = null);
        void Update(TEntity entity, IDbTransaction transaction = null);
        void Update(IList<TEntity> entities, IDbTransaction transaction = null);
        void UpdateBySp(TEntity entity, IDbTransaction transaction = null);
        void UpdateBySp(IList<TEntity> entities, IDbTransaction transaction = null);
        void Delete(TEntity entity, IDbTransaction transaction = null);
        void Delete(IList<TEntity> entities, IDbTransaction transaction = null);
        void DeleteBySp(TEntity entity, IDbTransaction transaction = null);
        void DeleteBySp(IList<TEntity> entities, IDbTransaction transaction = null);
        void DeleteBySp(IDbTransaction transaction = null, params  object[] id);
        void Delete(IDbTransaction transaction = null, params  object[] id);
        IEnumerable<TEntity> GetAll(IDbTransaction transaction = null, params string[] selectClause);
        IEnumerable<TEntity> GetAllBySp(IDbTransaction transaction = null);
        TEntity GetById(IDbTransaction transaction = null, string[] selectClause = null, params object[] id);
        TEntity GetByIdWithSp(IDbTransaction transaction = null, params object[] id);
        IEnumerable<object> ExecuteSqlFunction(string functionName, FunctionType functionType, IDbTransaction transaction = null, params string[] argumens);
    }
}
