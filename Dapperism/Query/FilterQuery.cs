using System;
using System.Linq;
using System.Linq.Expressions;
using Dapperism.Entities;
using Dapperism.Enums;
using Dapperism.Utilities;

namespace Dapperism.Query
{
    public class FilterQuery<TEntity> where TEntity : class, IEntity, new()
    {
        private string _tableName = "";
        private string _schemaName = "dbo";
        private bool _isDistinct;
        private string _selectCol = "";
        private string _qText = "";
        private bool _allAnd;
        private bool _allOr;
        private bool _allNot;
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

        public FilterQuery<TEntity> And(bool forAll = false)
        {
            _allAnd = forAll;
            _allOr = !forAll;
            _qText += " AND ";
            return this;
        }

        public FilterQuery<TEntity> Or(bool forAll = false)
        {
            _allAnd = !forAll;
            _allOr = forAll;
            _qText += " OR ";
            return this;
        }

        public FilterQuery<TEntity> Not(bool forAll = false)
        {
            _allOr = forAll;
            _qText += " NOT ";
            return this;
        }

        public FilterQuery<TEntity> Where(Expression<Func<TEntity, object>> field, FilterOperation filterOperation, object value)
        {
            var columnName = field.GetMemberName();
            /*var key = typeof(TEntity).FullName.Trim() + "." + name.Trim();
            var data = EntityAnalyser<TEntity>.GetInfo();
            var type = data.NotSeparatedInfo[key].PropertyType;*/

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











                case FilterOperation.Like:
                    break;
                case FilterOperation.MultipleLike:
                    break;
                case FilterOperation.MultipleTotalLike:
                    break;
                case FilterOperation.MultipleStartLike:
                    break;
                case FilterOperation.MultipleEndLike:
                    break;
                case FilterOperation.NotLike:
                    break;
                case FilterOperation.NotMultipleLike:
                    break;
                case FilterOperation.NotMultipleTotalLike:
                    break;
                case FilterOperation.NotMultipleStartLike:
                    break;
                case FilterOperation.NotMultipleEndLike:
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
                default:
                    throw new ArgumentOutOfRangeException("filterOperation");
            }


            return this;
        }


    }
}
