using System;
using System.Linq;
using System.Linq.Expressions;
using Dapperism.Entity;
using Dapperism.EntityAnalysis;
using Dapperism.Enums;
using Dapperism.Extensions.Extensions;
using Dapperism.Extensions.Persian;
using Dapperism.Extensions.Utilities;

namespace Dapperism.Query
{
    public class QueryExpression<TEntity> where TEntity : class, IEntity, new()
    {
        private string _tableName;
        private string _schemaName;
        private bool _isDistinct;
        private string _selectCol = "";
        private string _qText = "";
        internal string CreateQuery()
        {
            var key = typeof(TEntity).FullName.Trim();
            var ea = CacheManager.Instance[key] as EntityAttributes;
            if (ea == null)
                throw new Exception("type not found");
            _tableName = ea.TableName;
            _schemaName = ea.SchemaName;

            string str;
            if (string.IsNullOrEmpty(_qText))
                str = string.Format("SELECT {0} {1} FROM {2}.{3}", _isDistinct ? "DISTINCT" : "", _selectCol, _schemaName, _tableName);
            else
            {
                str = string.Format("SELECT {0} {1} FROM {2}.{3} WHERE {4}", _isDistinct ? "DISTINCT" : "", _selectCol, _schemaName, _tableName, _qText);
            }
            return str;
        }

        public QueryExpression<TEntity> Select(bool isDistinct = false, params string[] selectClause)
        {
            var select = "*";
            if (selectClause != null && selectClause.Any())
                select = string.Format("[{0}]", selectClause.Aggregate((a, b) => string.Format("{0}] , [{1}", a, b)));
            _selectCol = select;
            _isDistinct = isDistinct;

            return this;
        }

        public QueryExpression<TEntity> Begin
        {
            get
            {
                _qText += " ( ";
                return this;
            }
        }

        public QueryExpression<TEntity> End
        {
            get
            {
                _qText += " ) ";
                return this;
            }
        }

        public QueryExpression<TEntity> And
        {
            get
            {
                _qText += " AND ";
                return this;
            }
        }

        public QueryExpression<TEntity> Or
        {
            get
            {
                _qText += " OR ";
                return this;
            }
        }

        public QueryExpression<TEntity> Not
        {
            get
            {
                _qText += " NOT ";
                return this;
            }
        }

        public QueryExpression<TEntity> Where(Expression<Func<TEntity, object>> field, ConditionType filterOperation, object value)
        {
            var columnName = field.GetMemberName();

            switch (filterOperation)
            {
                case ConditionType.Equal:
                    var v1 = value as PersianDateTime;
                    if (value is DateTime)
                    {
                        var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "=", v);
                        break;
                    }
                    if (v1 != null)
                    {
                        var v = v1.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "=", v);
                        break;
                    }
                    _qText += string.Format("({0} {1} '{2}')", columnName, "=", value);
                    break;
                case ConditionType.NotEqual:
                    var v2 = value as PersianDateTime;
                    if (value is DateTime)
                    {
                        var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "!=", v);
                        break;
                    }
                    if (v2 != null)
                    {
                        var v = v2.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "!=", v);
                        break;
                    }
                    _qText += string.Format("({0} {1} '{2}')", columnName, "!=", value);
                    break;
                case ConditionType.GreaterThan:
                    var v3 = value as PersianDateTime;
                    if (value is DateTime)
                    {
                        var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, ">", v);
                        break;
                    }
                    if (v3 != null)
                    {
                        var v = v3.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, ">", v);
                        break;
                    }
                    _qText += string.Format("({0} {1} '{2}')", columnName, ">", value);
                    break;
                case ConditionType.GreaterThanEqual:
                    var v4 = value as PersianDateTime;
                    if (value is DateTime)
                    {
                        var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, ">=", v);
                        break;
                    }
                    if (v4 != null)
                    {
                        var v = v4.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, ">=", v);
                        break;
                    }
                    _qText += string.Format("({0} {1} '{2}')", columnName, ">=", value);
                    break;
                case ConditionType.LessThan:
                    var v5 = value as PersianDateTime;
                    if (value is DateTime)
                    {
                        var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "<", v);
                        break;
                    }
                    if (v5 != null)
                    {
                        var v = v5.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "<", v);
                        break;
                    }
                    _qText += string.Format("({0} {1} '{2}')", columnName, "<", value);
                    break;
                case ConditionType.LessThanEqual:
                    var v6 = value as PersianDateTime;
                    if (value is DateTime)
                    {
                        var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "<=", v);
                        break;
                    }
                    if (v6 != null)
                    {
                        var v = v6.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                        _qText += string.Format("({0} {1} '{2}')", columnName, "<=", v);
                        break;
                    }
                    _qText += string.Format("({0} {1} '{2}')", columnName, "<=", value);
                    break;

                case ConditionType.NotLike:
                    _qText += string.Format("({0} {1} N'%{2}%')", columnName, "NOT LIKE", value);
                    break;
                case ConditionType.Like:
                    _qText += string.Format("({0} {1} N'%{2}%')", columnName, "LIKE", value);
                    break;
                case ConditionType.StartsWith:
                    _qText += string.Format("({0} {1} N'{2}%')", columnName, "LIKE", value);
                    break;
                case ConditionType.DoesNotStartWith:
                    _qText += string.Format("({0} {1} N'{2}%')", columnName, "NOT LIKE", value);
                    break;
                case ConditionType.EndsWith:
                    _qText += string.Format("({0} {1} N'%{2}')", columnName, "LIKE", value);
                    break;
                case ConditionType.DoesNotEndWith:
                    _qText += string.Format("({0} {1} N'%{2}')", columnName, "NOT LIKE", value);
                    break;
                case ConditionType.IsNull:
                    _qText += string.Format("({0} {1})", columnName, "IS NULL");
                    break;
                case ConditionType.IsNotNull:
                    _qText += string.Format("({0} {1})", columnName, "IS NOT NULL");
                    break;
                case ConditionType.In:
                    _qText += string.Format("({0} {1} ({2}))", columnName, "IN", value);
                    break;
                case ConditionType.MultipleLike:
                    var value2 = value as string[];
                    if (value2 != null && value2.Any())
                    {
                        var v = "LIKE N'" + value2.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case ConditionType.MultipleTotalLike:
                    var strings2 = value as string[];
                    if (strings2 != null && strings2.Any())
                    {
                        var v = "LIKE N'%" + strings2.Aggregate((a, b) => a + "%" + b) + "%'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case ConditionType.MultipleStartLike:
                    var value3 = value as string[];
                    if (value3 != null && value3.Any())
                    {
                        var v = "LIKE N'%" + value3.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case ConditionType.MultipleEndLike:
                    var strings3 = value as string[];
                    if (strings3 != null && strings3.Any())
                    {
                        var v = "LIKE N'" + strings3.Aggregate((a, b) => a + "%" + b) + "%'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case ConditionType.NotMultipleTotalLike:
                    var strings = value as string[];
                    if (strings != null && strings.Any())
                    {
                        var v = "NOT LIKE N'%" + strings.Aggregate((a, b) => a + "%" + b) + "%'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case ConditionType.NotMultipleLike:
                    var value4 = value as string[];
                    if (value4 != null && value4.Any())
                    {
                        var v = "NOT LIKE N'" + value4.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case ConditionType.NotMultipleStartLike:
                    var strings1 = value as string[];
                    if (strings1 != null && strings1.Any())
                    {
                        var v = "NOT LIKE N'%" + strings1.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case ConditionType.NotMultipleEndLike:
                    var value1 = value as string[];
                    if (value1 != null && value1.Any())
                    {
                        var v = "NOT LIKE N'" + value1.Aggregate((a, b) => a + "%" + b) + "%'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException("filterOperation");
            }


            return this;
        }


    }
}
