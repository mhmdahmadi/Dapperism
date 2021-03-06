﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using Dapper;
using Dapperism.Entity;
using Dapperism.EntityAnalysis;
using Dapperism.Enums;
using Dapperism.Extensions.Extensions;
using Dapperism.Extensions.Utilities;
using Dapperism.Query;
using Dapperism.Validation;
using DynamicParameters = Dapper.DynamicParameters;

namespace Dapperism.DataAccess
{
    public sealed class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
    {
        private IDbConnection _dbConnection;
        private DbProviderFactory _providerFactory;
        private string _providerName;
        private readonly EntityAttributes _entityAttributes;
        public ValidationResults ValidationResults { get; set; }

        public Repository()
        {
            _entityAttributes = EntityAnalyser<TEntity>.GetInfo();
        }

        public Repository(string cnnStringName)
        {
            _entityAttributes = EntityAnalyser<TEntity>.GetInfo();
            ConnectionStringName = cnnStringName;
        }

        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString; }
        }

        private DbProviderFactory ProviderFactory
        {
            get
            {
                var providerName = ProviderName;
                if (_providerFactory != null)
                {
                    return _providerFactory;
                }

                _providerFactory = DbProviderFactories.GetFactory(providerName);
                return _providerFactory;

            }
        }

        private string ConnectionStringName { get; set; }

        private string ProviderName
        {
            get
            {
                if (_providerName != null)
                    return _providerName;

                if (string.IsNullOrEmpty(ConnectionStringName))
                    ConnectionStringName = "dapperismdb";

                _providerName = ConfigurationManager.ConnectionStrings[ConnectionStringName].ProviderName;
                return _providerName;
            }
        }

        private IDbConnection DbConnection
        {
            get
            {

                if (_dbConnection != null)
                {
                    return _dbConnection;
                }

                _dbConnection = ProviderFactory.CreateConnection();
                return _dbConnection;
            }
        }

        /*private IDbCommand DbCommand
        {
            get
            {
                return _dbConnection.CreateCommand();
            }
        }*/
        private void OpenDatabase()
        {
            if (DbConnection.State == ConnectionState.Open) return;
            DbConnection.ConnectionString = ConnectionString;
            DbConnection.Open();
        }
        private void CloseDatabase()
        {
            if (DbConnection.State != ConnectionState.Closed)
                DbConnection.Close();
        }
        public IDbTransaction BeginTransaction()
        {
            OpenDatabase();
            var trans = DbConnection.BeginTransaction();
            return trans;
        }
        public void CommitTransaction(IDbTransaction transaction)
        {
            transaction.Commit();
            CloseDatabase();
        }
        public void RollbackTransaction(IDbTransaction transaction)
        {
            transaction.Rollback();
            CloseDatabase();
        }
        public TEntity Insert(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null)
        {
            if (entity == null) return null;

            if (_entityAttributes.RetrieveOnly)
                throw new Exception("Class is defined as [RetrieveOnly]");

            ValidationResults = null;
            if (!entity.IsValid())
            {
                ValidationResults = entity.Validate();
                return null;
            }

            switch (method)
            {
                case MethodType.Text:

                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            object[] obj = EntityAnalyser<TEntity>.GetValues(entity);
                            var cmdText = _entityAttributes.HaveAutoNumber
                                ? _entityAttributes.InsertReturnStatement
                                : _entityAttributes.InsertStatement;
                            var cmd = string.Format(cmdText, obj);
                            var result =
                                DbConnection.Query<TEntity>(cmd, transaction: trans, commandType: CommandType.Text)
                                    .FirstOrDefault();
                            if (transaction == null)
                                CommitTransaction(trans);
                            return result;
                        }
                    }
                case MethodType.StoredProcedure:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            TEntity result = entity;
                            var data =
                                EntityAnalyser<TEntity>.GetData(entity)
                                    .Where(x => !x.IsViewColumn && x.IsSpCudParameter)
                                    .ToList();
                            var spName = _entityAttributes.InsertSpName;

                            var param = new DynamicParameters();
                            foreach (var d in data)
                                param.Add(d.SpParamName, d.Value, d.ParameterType, d.ParameterDirection);

                            DbConnection.Execute(spName, param, trans, commandType: CommandType.StoredProcedure);

                            var auto = data.FirstOrDefault(x => x.AutoNumber == 1);
                            if (auto != null)
                            {
                                var autoNum = auto.SpParamName;
                                var id = param.Get<dynamic>(autoNum);
                                ReflectionManager.SetPropertyValue(entity, auto.PropertyName, id);
                                result = entity;
                            }

                            if (transaction == null)
                                CommitTransaction(trans);

                            return result;
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }
        public void Insert(IList<TEntity> entities, MethodType method = MethodType.Text, IDbTransaction transaction = null)
        {
            if (entities == null) return;
            if (_entityAttributes.RetrieveOnly)
                throw new Exception("Class is defined as [RetrieveOnly]");

            ValidationResults = null;
            var c = entities.Count();
            var cmd = new string[c];

            switch (method)
            {
                case MethodType.Text:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            var ins = _entityAttributes.HaveAutoNumber
                                ? _entityAttributes.InsertReturnStatement
                                : _entityAttributes.InsertStatement;
                            int i = 0;
                            foreach (var entity in entities)
                            {
                                if (!entity.IsValid())
                                {
                                    var cascadeMode = _entityAttributes.CascadeMode;
                                    switch (cascadeMode)
                                    {
                                        case CascadeMode.Continue:
                                            continue;
                                        case CascadeMode.StopOnFirstFailure:
                                            return;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }

                                object[] obj = EntityAnalyser<TEntity>.GetValues(entity);
                                cmd[i] = string.Format(ins, obj);
                                i++;
                            }
                            var exeCmd = cmd.Aggregate((a, b) => a + " " + b);
                            DbConnection.Execute(exeCmd, transaction: trans, commandType: CommandType.Text);
                            if (transaction == null)
                                CommitTransaction(trans);
                        }
                    }
                    break;
                case MethodType.StoredProcedure:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            foreach (var entity in entities)
                            {
                                if (!entity.IsValid())
                                {
                                    var cascadeMode = _entityAttributes.CascadeMode;
                                    switch (cascadeMode)
                                    {
                                        case CascadeMode.Continue:
                                            continue;
                                        case CascadeMode.StopOnFirstFailure:
                                            return;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }
                                var data =
                                    EntityAnalyser<TEntity>.GetData(entity)
                                        .Where(x => !x.IsViewColumn && x.IsSpCudParameter)
                                        .ToList();
                                var spName = _entityAttributes.InsertSpName;
                                var param = new DynamicParameters();
                                foreach (var d in data)
                                    param.Add(d.SpParamName, d.Value, d.ParameterType, d.ParameterDirection);
                                DbConnection.Execute(spName, param, trans, commandType: CommandType.StoredProcedure);
                            }
                            if (transaction == null)
                                CommitTransaction(trans);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }
        public void Update(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null)
        {
            if (entity == null) return;
            if (_entityAttributes.RetrieveOnly)
                throw new Exception("Class is defined as [RetrieveOnly]");
            ValidationResults = null;
            if (!entity.IsValid())
            {
                ValidationResults = entity.Validate();
                return;
            }

            switch (method)
            {
                case MethodType.Text:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            object[] obj = EntityAnalyser<TEntity>.GetValues(entity);
                            var cmdText = string.Format(_entityAttributes.UpdateStatement, obj);
                            var r = DbConnection.Execute(cmdText, transaction: trans, commandType: CommandType.Text);
                            var result = r >= 0;

                            if (transaction == null)
                                CommitTransaction(trans);
                        }
                    }
                    break;
                case MethodType.StoredProcedure:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            var data =
                                EntityAnalyser<TEntity>.GetData(entity)
                                    .Where(x => !x.IsViewColumn && x.IsSpCudParameter)
                                    .ToList();
                            var spName = _entityAttributes.UpdateSpName;

                            var param = new DynamicParameters();
                            foreach (var d in data)
                                param.Add(d.SpParamName, d.Value, d.ParameterType, ParameterDirection.Input);

                            DbConnection.Execute(spName, param, trans, commandType: CommandType.StoredProcedure);
                            if (transaction == null)
                                CommitTransaction(trans);

                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }
        public void Update(IList<TEntity> entities, MethodType method = MethodType.Text, IDbTransaction transaction = null)
        {
            if (entities == null) return;
            if (_entityAttributes.RetrieveOnly)
                throw new Exception("Class is defined as [RetrieveOnly]");
            ValidationResults = null;
            var c = entities.Count();
            var cmd = new string[c];


            switch (method)
            {
                case MethodType.Text:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            int i = 0;
                            foreach (var entity in entities)
                            {
                                if (!entity.IsValid())
                                {
                                    var cascadeMode = _entityAttributes.CascadeMode;
                                    switch (cascadeMode)
                                    {
                                        case CascadeMode.Continue:
                                            continue;
                                        case CascadeMode.StopOnFirstFailure:
                                            return;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }

                                object[] obj = EntityAnalyser<TEntity>.GetValues(entity);
                                var cmdText = string.Format(_entityAttributes.UpdateStatement, obj);
                                cmd[i] = cmdText;
                                i++;
                            }
                            var exeCmd = cmd.Aggregate((a, b) => a + " " + b);
                            DbConnection.Execute(exeCmd, transaction: trans, commandType: CommandType.Text);
                            if (transaction == null)
                                CommitTransaction(trans);
                        }
                    }
                    break;
                case MethodType.StoredProcedure:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            foreach (var entity in entities)
                            {
                                if (!entity.IsValid())
                                {
                                    var cascadeMode = _entityAttributes.CascadeMode;
                                    switch (cascadeMode)
                                    {
                                        case CascadeMode.Continue:
                                            continue;
                                        case CascadeMode.StopOnFirstFailure:
                                            return;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                }
                                var data =
                                    EntityAnalyser<TEntity>.GetData(entity)
                                        .Where(x => !x.IsViewColumn && x.IsSpCudParameter)
                                        .ToList();
                                var spName = _entityAttributes.UpdateSpName;
                                var param = new DynamicParameters();
                                foreach (var d in data)
                                    param.Add(d.SpParamName, d.Value, d.ParameterType, ParameterDirection.Input);

                                DbConnection.Execute(spName, param, trans, commandType: CommandType.StoredProcedure);
                            }
                            if (transaction == null)
                                CommitTransaction(trans);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }
        public TEntity InsertOrUpdate(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null)
        {
            if (entity == null) return null;

            if (_entityAttributes.RetrieveOnly)
                throw new Exception("Class is defined as [RetrieveOnly]");

            ValidationResults = null;
            if (!entity.IsValid())
            {
                ValidationResults = entity.Validate();
                return null;
            }
            object[] obj = EntityAnalyser<TEntity>.GetValues(entity);
            var cmdtxt = _entityAttributes.HaveAutoNumber
                ? _entityAttributes.InsertReturnStatement
                : _entityAttributes.InsertStatement;
            var cmdInsert = string.Format(cmdtxt, obj);
            var cmdUpdate = string.Format(_entityAttributes.UpdateStatement, obj);
            var cmdSelectId = string.Format(_entityAttributes.SelectByIdStatement, obj);
            var cmdWhere = string.Format(_entityAttributes.WhereStatement, obj);
            switch (method)
            {

                case MethodType.Text:
                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {

                            var cmdText = string.Format("IF EXISTS (SELECT * FROM {0}.{1} WHERE {2}) BEGIN {3} {4} END ELSE BEGIN {5} END",
                                _entityAttributes.SchemaName, _entityAttributes.TableName, cmdWhere,
                                cmdUpdate, cmdSelectId, cmdInsert);

                            var result =
                                DbConnection.Query<TEntity>(cmdText, transaction: trans, commandType: CommandType.Text)
                                    .FirstOrDefault();
                            if (transaction == null)
                                CommitTransaction(trans);
                            return result;
                        }
                    }
                case MethodType.StoredProcedure:

                    using (DbConnection)
                    {
                        var trans = transaction ?? BeginTransaction();
                        using (trans)
                        {
                            TEntity result;
                            var exists = DbConnection.Query<bool>(string.Format("SELECT TOP 1 1 FROM {0}.{1} WHERE {2}",
                                _entityAttributes.SchemaName, _entityAttributes.TableName, cmdWhere),
                                transaction: trans, commandType: CommandType.Text).FirstOrDefault();
                            if (exists)
                            {
                                Update(entity, MethodType.StoredProcedure, trans);
                                result = entity;
                            }
                            else
                                result = Insert(entity, MethodType.StoredProcedure, trans);

                            if (transaction == null)
                                CommitTransaction(trans);
                            return result;
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException("method");
            }
        }
        public void Delete(TEntity entity, MethodType method = MethodType.Text, IDbTransaction transaction = null)
        {
            if (entity == null) return;
            if (_entityAttributes.RetrieveOnly)
                throw new Exception("Class is defined as [RetrieveOnly]");
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    switch (method)
                    {
                        case MethodType.Text:

                            object[] obj = EntityAnalyser<TEntity>.GetValues(entity);
                            var cmd = string.Format(_entityAttributes.DeleteStatement, obj);
                            DbConnection.Execute(cmd, transaction: trans, commandType: CommandType.Text);
                            if (transaction == null)
                                CommitTransaction(trans);
                            break;
                        case MethodType.StoredProcedure:
                            var data = EntityAnalyser<TEntity>.GetData(entity);
                            var spName = _entityAttributes.DeleteSpName;
                            var ids = data.Where(x => x.AutoNumber != -1).ToList();
                            var param = new DynamicParameters();
                            foreach (var id in ids)
                                param.Add(id.SpParamName, id.Value, id.ParameterType, ParameterDirection.Input);
                            DbConnection.Execute(spName, param, trans, commandType: CommandType.StoredProcedure);
                            if (transaction == null)
                                CommitTransaction(trans);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("method");
                    }
                }
            }
        }
        public void Delete(IList<TEntity> entities, MethodType method = MethodType.Text, IDbTransaction transaction = null)
        {
            if (entities == null) return;
            if (_entityAttributes.RetrieveOnly)
                throw new Exception("Class is defined as [RetrieveOnly]");
            ValidationResults = null;
            var c = entities.Count();
            var cmd = new string[c];
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    switch (method)
                    {
                        case MethodType.Text:
                            int i = 0;
                            foreach (var entity in entities)
                            {
                                object[] obj = EntityAnalyser<TEntity>.GetValues(entity);
                                var cmdText = string.Format(_entityAttributes.UpdateStatement, obj);

                                cmd[i] = cmdText;
                                i++;
                            }
                            var exeCmd = cmd.Aggregate((a, b) => a + " " + b);
                            DbConnection.Execute(exeCmd, transaction: trans, commandType: CommandType.Text);
                            if (transaction == null)
                                CommitTransaction(trans);
                            break;
                        case MethodType.StoredProcedure:
                            var spName = _entityAttributes.DeleteSpName;
                            foreach (var entity in entities)
                            {
                                var data = EntityAnalyser<TEntity>.GetData(entity);
                                var ids = data.Where(x => x.AutoNumber != -1).ToList();

                                var param = new DynamicParameters();
                                foreach (var id in ids)
                                    param.Add(id.SpParamName, id.Value, id.ParameterType, ParameterDirection.Input);

                                DbConnection.Execute(spName, param, trans, commandType: CommandType.StoredProcedure);
                            }
                            if (transaction == null)
                                CommitTransaction(trans);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("method");
                    }
                }
            }
        }
        public void Delete(MethodType method = MethodType.Text, IDbTransaction transaction = null, params object[] id)
        {
            if (id == null) return;
            if (_entityAttributes.RetrieveOnly)
                return;
            var ids = _entityAttributes.PrimaryKeys;
            if (ids.Count() != id.Count())
                throw new Exception("Id`s count not matched");
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    switch (method)
                    {
                        case MethodType.Text:

                            string str = "";
                            int i = 0;
                            foreach (var d in ids)
                            {
                                str += string.Format("[{0}] = '{1}' ", d.Item1, id[i]);
                                i++;
                            }
                            var whereStr = str.Replace("'[", "' AND [");
                            var cmdText = string.Format("DELETE FROM {0}.{1} WHERE {2}", _entityAttributes.SchemaName,
                                _entityAttributes.TableName, whereStr.Trim());
                            DbConnection.Execute(cmdText, transaction: trans, commandType: CommandType.Text);
                            if (transaction == null)
                                CommitTransaction(trans);

                            break;
                        case MethodType.StoredProcedure:
                            string spName = _entityAttributes.DeleteSpName;
                            var param = new DynamicParameters();
                            int j = 0;
                            foreach (var d in ids)
                            {
                                param.Add(d.Item2, id[j], d.Item3, ParameterDirection.Input);
                                j++;
                            }

                            DbConnection.Execute(spName, param, trans, commandType: CommandType.StoredProcedure);
                            if (transaction == null)
                                CommitTransaction(trans);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException("method");
                    }


                }
            }

        }
        public IEnumerable<TEntity> GetAll(int? pageIndex = null, int? pageSize = null, string[] pagingOrderCols = null,
            bool? isAscendingOrder = null, IDbTransaction transaction = null, params string[] selectClause)
        {
            if (pageIndex != null && pageIndex < 1)
                throw new ArgumentOutOfRangeException("pageIndex must be greater than zero");

            if (pageSize != null && pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize must be greater than zero");

            bool isAsc = isAscendingOrder ?? true;

            string isAscStr = isAsc ? " ASC " : " DESC ";



            var select = "*";
            if (selectClause.Any() && selectClause != null)
            {
                select = string.Format("[{0}]", selectClause.Aggregate((a, b) => string.Format("{0}] , [{1}", a, b)));
                //select = select.Replace("[[", "[").Replace("]]", "]");
            }



            var svt = string.IsNullOrEmpty(_entityAttributes.ViewName)
                ? string.Format("{0}.{1}", _entityAttributes.SchemaName, _entityAttributes.TableName)
                : string.Format("{0}.{1}", _entityAttributes.SchemaName, _entityAttributes.ViewName);

            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    IEnumerable<TEntity> result;
                    if (pageIndex != null && pageSize != null)
                    {
                        string order;
                        if (pagingOrderCols != null && pagingOrderCols.Any())
                            order = string.Format("[{0}]",
                                pagingOrderCols.Aggregate((a, b) => string.Format("{0} {2}] , [{1} {2}", a, b, isAscStr)));
                        else
                        {
                            var pkLst = _entityAttributes.PrimaryKeys.Select(x => x.Item1);
                            order =
                                string.Format("[{0}]", pkLst.Aggregate((a, b) => string.Format("{0}] , [{1}", a, b)))
                                    .Replace("]", "] " + isAscStr);
                        }
                        var paging = string.Format(
                            "SELECT {0} FROM ( SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS ROWNUMBER, {0} FROM {2} ) AS TBL WHERE ROWNUMBER BETWEEN (({3} - 1) * {4} + 1) AND ({3} * {4})",
                            select, order, svt, pageIndex, pageSize);
                        result = DbConnection.Query<TEntity>(paging, transaction: trans, commandType: CommandType.Text);
                    }
                    else
                        result = DbConnection.Query<TEntity>(string.Format("SELECT {0} FROM {1}", select, svt),
                            transaction: trans, commandType: CommandType.Text);
                    if (transaction == null)
                        CommitTransaction(trans);
                    return result;
                }
            }
        }
        public IEnumerable<TEntity> GetAllBySp(IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var spName = _entityAttributes.SelectAllSpName;

                    var result = DbConnection.Query<TEntity>(spName, transaction: trans,
                        commandType: CommandType.StoredProcedure);

                    if (transaction == null)
                        CommitTransaction(trans);
                    return result;
                }

            }
        }
        public TEntity GetById(MethodType method = MethodType.Text, IDbTransaction transaction = null, string[] selectClause = null, params object[] id)
        {
            TEntity result = null;
            var ids = _entityAttributes.PrimaryKeys;
            string str = "";

            if (ids.Count() != id.Count())
                throw new Exception("Id`s count not matched");
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {

                    switch (method)
                    {
                        case MethodType.Text:
                            var select = "*";
                            if (selectClause != null && selectClause.Any())
                                select = string.Format("[{0}]", selectClause.Aggregate((a, b) => string.Format("{0}] , [{1}", a, b)));

                            var vt = string.IsNullOrEmpty(_entityAttributes.ViewName)
                                ? _entityAttributes.TableName
                                : _entityAttributes.ViewName;
                            int i = 0;
                            foreach (var d in ids)
                            {
                                str += string.Format("[{0}] = '{1}' ", d.Item1, id[i]);
                                i++;
                            }
                            var whereStr = str.Replace("'[", "' AND [");

                            result = DbConnection.Query<TEntity>(
                                                      string.Format("SELECT {0} FROM {1}.{2} WHERE {3}", select, _entityAttributes.SchemaName,
                                                          vt, whereStr), transaction: trans, commandType: CommandType.Text).FirstOrDefault();

                            if (transaction == null)
                                CommitTransaction(trans);

                            break;
                        case MethodType.StoredProcedure:

                            var spName = _entityAttributes.SelectByIdSpName;
                            var param = new DynamicParameters();
                            int j = 0;
                            foreach (var d in ids)
                            {
                                param.Add(d.Item2, id[j], d.Item3, ParameterDirection.Input);
                                j++;
                            }
                            result =
                               DbConnection.Query<TEntity>(spName, param, trans, commandType: CommandType.StoredProcedure)
                                   .FirstOrDefault();

                            if (transaction == null)
                                CommitTransaction(trans);
                            return result;
                        default:
                            throw new ArgumentOutOfRangeException("method");
                    }
                    return result;
                }
            }
        }

        public IEnumerable<dynamic> ExecSqlTableFunction(string functionName, IDbTransaction transaction = null,
            string[] selectClause = null, params string[] argumens)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var select = "*";
                    if (selectClause != null && selectClause.Any())
                        select = string.Format("[{0}]",
                            selectClause.Aggregate((a, b) => string.Format("{0}] , [{1}", a, b)));

                    var args = "";
                    if (argumens != null && argumens.Any())
                        args = ("'" + argumens.Aggregate((a, b) => a + "' , '" + b) + "'").Trim();
                    var result =
                        DbConnection.Query<dynamic>(
                            string.Format("Select {0} From {1}({2})", select, functionName, args), transaction: trans,
                            commandType: CommandType.Text);
                    if (transaction == null)
                        CommitTransaction(trans);

                    return result;
                }
            }
        }

        public dynamic ExecSqlScalarFunction(string functionName, IDbTransaction transaction = null,
            params string[] argumens)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var args = "";
                    if (argumens != null && argumens.Any())
                        args = ("'" + argumens.Aggregate((a, b) => a + "' , '" + b) + "'").Trim();

                    var result =
                        DbConnection.Query<dynamic>(string.Format("Select {0}({1}) AS Result", functionName, args),
                            transaction: trans, commandType: CommandType.Text).FirstOrDefault();

                    if (transaction == null)
                        CommitTransaction(trans);

                    return result;
                }
            }
        }

        public IEnumerable<TEntity> ExecStoredProcedure(string spName, object param = null,
            IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var result = DbConnection.Query<TEntity>(spName, param, trans,
                        commandType: CommandType.StoredProcedure);

                    if (transaction == null)
                        CommitTransaction(trans);

                    return result;
                }
            }
        }

        public IEnumerable<TEntity> ExecStoredProcedure(string spName,
            DynamicParameters dynamicParams, IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var dp = dynamicParams.ToDapperParams();

                    var result = DbConnection.Query<TEntity>(spName, dp, trans,
                        commandType: CommandType.StoredProcedure);

                    if (transaction == null)
                        CommitTransaction(trans);

                    return result;
                }
            }
        }

        public IEnumerable<dynamic> ExecDynamicStoredProcedure(string spName, object param = null,
            IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var result = DbConnection.Query<dynamic>(spName, param, trans,
                        commandType: CommandType.StoredProcedure);

                    if (transaction == null)
                        CommitTransaction(trans);

                    return result;
                }
            }
        }

        public IEnumerable<dynamic> ExecDynamicStoredProcedure(string spName,
            DynamicParameters dynamicParams, IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var dp = dynamicParams.ToDapperParams();

                    var result = DbConnection.Query<dynamic>(spName, dp, trans,
                        commandType: CommandType.StoredProcedure);

                    if (transaction == null)
                        CommitTransaction(trans);

                    return result;
                }
            }
        }

        public IEnumerable<TEntity> GetByFilter(QueryExpression<TEntity> query, IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var q = query.CreateQuery();
                    var r = DbConnection.Query<TEntity>(q, commandType: CommandType.Text, transaction: trans);

                    if (transaction == null)
                        CommitTransaction(trans);

                    return r;
                }
            }
        }

        public decimal Count(IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var r = DbConnection.Query<decimal>(string.Format("SELECT Count(*) FROM {0} As Result", _entityAttributes.STVCombination), commandType: CommandType.Text, transaction: trans).FirstOrDefault();
                    if (transaction == null)
                        CommitTransaction(trans);

                    return r;
                }
            }
        }

        public decimal Count(QueryExpression<TEntity> query, IDbTransaction transaction = null)
        {
            using (DbConnection)
            {
                var trans = transaction ?? BeginTransaction();
                using (trans)
                {
                    var q = query.CreateQuery();
                    var r = DbConnection.Query<decimal>(string.Format("SELECT Count(*) FROM ({0}) As Result", q), commandType: CommandType.Text, transaction: trans).FirstOrDefault();
                    if (transaction == null)
                        CommitTransaction(trans);

                    return r;
                }
            }
        }



    }
}