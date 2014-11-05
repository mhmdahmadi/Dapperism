using System;
using System.Linq;
using System.Linq.Expressions;
using Dapperism.Entities;
using Dapperism.Enums;
using Dapperism.Extensions.Extensions;
using Dapperism.Extensions.Persian;
using Dapperism.Extensions.Utilities;
using Dapperism.Utilities;

namespace Dapperism.Query
{
    public class FilterQuery<TEntity> where TEntity : class, IEntity, new()
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

        public FilterQuery<TEntity> Select(bool isDistinct = false, params string[] selectClause)
        {
            var select = "*";
            if (selectClause != null && selectClause.Any())
                select = string.Format("[{0}]", selectClause.Aggregate((a, b) => string.Format("{0}] , [{1}", a, b)));
            _selectCol = select;
            _isDistinct = isDistinct;

            return this;
        }

        public FilterQuery<TEntity> Begin
        {
            get
            {
                _qText += " ( ";
                return this;
            }
        }

        public FilterQuery<TEntity> End
        {
            get
            {
                _qText += " ) ";
                return this;
            }
        }

        public FilterQuery<TEntity> And
        {
            get
            {
                _qText += " AND ";
                return this;
            }
        }

        public FilterQuery<TEntity> Or
        {
            get
            {
                _qText += " OR ";
                return this;
            }
        }

        public FilterQuery<TEntity> Not
        {
            get
            {
                _qText += " NOT ";
                return this;
            }
        }

        public FilterQuery<TEntity> Where(Expression<Func<TEntity, object>> field, FilterOperation filterOperation, object value)
        {
            var columnName = field.GetMemberName();

            switch (filterOperation)
            {
                case FilterOperation.Equal:
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
                case FilterOperation.NotEqual:
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
                case FilterOperation.GreaterThan:
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
                case FilterOperation.GreaterThanEqual:
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
                case FilterOperation.LessThan:
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
                case FilterOperation.LessThanEqual:
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

                case FilterOperation.NotLike:
                    _qText += string.Format("({0} {1} N'%{2}%')", columnName, "NOT LIKE", value);
                    break;
                case FilterOperation.Like:
                    _qText += string.Format("({0} {1} N'%{2}%')", columnName, "LIKE", value);
                    break;
                case FilterOperation.StartsWith:
                    _qText += string.Format("({0} {1} N'{2}%')", columnName, "LIKE", value);
                    break;
                case FilterOperation.DoesNotStartWith:
                    _qText += string.Format("({0} {1} N'{2}%')", columnName, "NOT LIKE", value);
                    break;
                case FilterOperation.EndsWith:
                    _qText += string.Format("({0} {1} N'%{2}')", columnName, "LIKE", value);
                    break;
                case FilterOperation.DoesNotEndWith:
                    _qText += string.Format("({0} {1} N'%{2}')", columnName, "NOT LIKE", value);
                    break;
                case FilterOperation.IsNull:
                    _qText += string.Format("({0} {1})", columnName, "IS NULL");
                    break;
                case FilterOperation.IsNotNull:
                    _qText += string.Format("({0} {1})", columnName, "IS NOT NULL");
                    break;
                case FilterOperation.In:
                    _qText += string.Format("({0} {1} ({2}))", columnName, "IN", value);
                    break;
                case FilterOperation.MultipleLike:
                    var value2 = value as string[];
                    if (value2 != null && value2.Any())
                    {
                        var v = "LIKE N'" + value2.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case FilterOperation.MultipleTotalLike:
                    var strings2 = value as string[];
                    if (strings2 != null && strings2.Any())
                    {
                        var v = "LIKE N'%" + strings2.Aggregate((a, b) => a + "%" + b) + "%'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case FilterOperation.MultipleStartLike:
                    var value3 = value as string[];
                    if (value3 != null && value3.Any())
                    {
                        var v = "LIKE N'%" + value3.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case FilterOperation.MultipleEndLike:
                    var strings3 = value as string[];
                    if (strings3 != null && strings3.Any())
                    {
                        var v = "LIKE N'" + strings3.Aggregate((a, b) => a + "%" + b) + "%'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case FilterOperation.NotMultipleTotalLike:
                    var strings = value as string[];
                    if (strings != null && strings.Any())
                    {
                        var v = "NOT LIKE N'%" + strings.Aggregate((a, b) => a + "%" + b) + "%'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case FilterOperation.NotMultipleLike:
                    var value4 = value as string[];
                    if (value4 != null && value4.Any())
                    {
                        var v = "NOT LIKE N'" + value4.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case FilterOperation.NotMultipleStartLike:
                    var strings1 = value as string[];
                    if (strings1 != null && strings1.Any())
                    {
                        var v = "NOT LIKE N'%" + strings1.Aggregate((a, b) => a + "%" + b) + "'";
                        _qText += string.Format("({0} {1})", columnName, v);
                    }
                    break;

                case FilterOperation.NotMultipleEndLike:
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
