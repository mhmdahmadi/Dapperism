using System;
using System.Linq;
using Dapperism.Enums;
using Dapperism.Utilities;

namespace Dapperism.Query
{
    public class FilterQuery2
    {
        public FilterQuery2()
        {
            QueryText = null;
        }

        private static string QueryText { get; set; }
        private static string TableName { get; set; }
        private static bool IsDistinct { get; set; }
        private static bool IsOrAggregation { get; set; }
        private bool _create;

        internal string GetQuery()
        {
            var result = "";
            if (string.IsNullOrEmpty(QueryText))
            {
                result = string.Format("SELECT {0} * FROM {1}", IsDistinct ? "DISTINCT" : "", TableName);
            }
            else
            {
                result = string.Format("SELECT {0} * FROM {1} WHERE {2}", IsDistinct ? "DISTINCT" : "", TableName, IsOrAggregation ? QueryText.Replace(")(", ") OR (") : QueryText.Replace(")(", ") AND ("));
            }
            return result;
        }

        //public FilterQuery Create()
        //{
        //    _create = true;
        //    return this;
        //}

        public FilterQuery2 Distinct
        {
            get
            {
                if (_create) IsDistinct = true;
                return this;
            }
        }

        public FilterQuery2 Begin
        {
            get
            {
                if (_create) QueryText += "(";
                return this;
            }
        }

        public FilterQuery2 End
        {
            get
            {
                if (_create) QueryText += ")";
                return this;
            }
        }

        public FilterQuery2 And
        {
            get
            {
                if (_create) QueryText += " AND ";
                return this;
            }
        }

        public FilterQuery2 Or
        {
            get
            {
                if (_create) QueryText += " OR ";
                return this;
            }
        }

        public FilterQuery2 Not
        {
            get
            {

                if (_create) QueryText += " NOT ";
                return this;
            }

        }

        public FilterQuery2 TableOrView(string name)
        {
            _create = true;
            TableName = name;
            return this;

        }

        public FilterQuery2 AggregateByOr
        {
            get
            {

                if (_create) IsOrAggregation = true;
                return this;
            }

        }

        public FilterQuery2 AddFilter(string columnName, FilterOperation filterOperation, object value)
        {

            if (_create)
            {
                switch (filterOperation)
                {
                    case FilterOperation.Equal:
                        QueryText += string.Format("({0} {1} {2})", columnName, "=", value);
                        break;
                    case FilterOperation.NotEqual:
                        QueryText += string.Format("({0} {1} {2})", columnName, "!=", value);
                        break;
                    case FilterOperation.GreaterThan:
                        QueryText += string.Format("({0} {1} {2})", columnName, ">", value);
                        break;
                    case FilterOperation.GreaterThanEqual:
                        QueryText += string.Format("({0} {1} {2})", columnName, ">=", value);
                        break;
                    case FilterOperation.LessThan:
                        QueryText += string.Format("({0} {1} {2})", columnName, "<", value);
                        break;
                    case FilterOperation.LessThanEqual:
                        QueryText += string.Format("({0} {1} {2})", columnName, "<=", value);
                        break;
                    case FilterOperation.Like:
                        QueryText += string.Format("({0} {1} '%{2}%')", columnName, "LIKE", value);
                        break;

                    case FilterOperation.MultipleLike:
                        if (value is string[])
                        {
                            var v = "LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.MultipleTotalLike:
                        if (value is string[])
                        {
                            var v = "LIKE '%" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.MultipleStartLike:
                        if (value is string[])
                        {
                            var v = "LIKE '%" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.MultipleEndLike:
                        if (value is string[])
                        {
                            var v = "LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleTotalLike:
                        var strings = value as string[];
                        if (strings != null)
                        {
                            var v = "NOT LIKE '%" + strings.Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleLike:
                        if (value is string[])
                        {
                            var v = "NOT LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleStartLike:
                        if (value is string[])
                        {
                            var v = "NOT LIKE '%" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleEndLike:
                        if (value is string[])
                        {
                            var v = "NOT LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", columnName, v);
                        }
                        break;

                    case FilterOperation.NotLike:
                        QueryText += string.Format("({0} {1} '%{2}%')", columnName, "NOT LIKE", value);
                        break;
                    case FilterOperation.StartsWith:
                        QueryText += string.Format("({0} {1} '{2}%')", columnName, "LIKE", value);
                        break;
                    case FilterOperation.DoesNotStartWith:
                        QueryText += string.Format("({0} {1} '{2}%')", columnName, "NOT LIKE", value);
                        break;
                    case FilterOperation.EndsWith:
                        QueryText += string.Format("({0} {1} '%{2}')", columnName, "LIKE", value);
                        break;
                    case FilterOperation.DoesNotEndWith:
                        QueryText += string.Format("({0} {1} '%{2}')", columnName, "NOT LIKE", value);
                        break;
                    case FilterOperation.IsNull:
                        QueryText += string.Format("({0} {1})", columnName, "IS NULL");
                        break;
                    case FilterOperation.IsNotNull:
                        QueryText += string.Format("({0} {1})", columnName, "IS NOT NULL");
                        break;
                    case FilterOperation.In:
                        QueryText += string.Format("({0} {1} ({2}))", columnName, "IN", value);
                        break;


                        /*
                    case FilterOperation.EqualDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, "=", v);
                        }

                        break;
                    case FilterOperation.GreaterThanDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, ">", v);
                        }
                        break;
                    case FilterOperation.LessThanDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, "<", v);
                        }

                        break;
                    case FilterOperation.GreaterThanEqualDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, ">=", v);
                        }
                        break;
                    case FilterOperation.LessThanEqualDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, "<=", v);
                        }
                        break;
                    case FilterOperation.EqualPersianDateTime:
                        var time = value as PersianDateTime;
                        if (time != null)
                        {
                            var v = time.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, "=", v);
                        }
                        break;
                    case FilterOperation.GreaterThanPersianDateTime:
                        var dateTime = value as PersianDateTime;
                        if (dateTime != null)
                        {
                            var v = dateTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, ">", v);
                        }
                        break;
                    case FilterOperation.LessThanPersianDateTime:
                        var persianDateTime = value as PersianDateTime;
                        if (persianDateTime != null)
                        {
                            var v = persianDateTime.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, "<", v);
                        }
                        break;
                    case FilterOperation.GreaterThanEqualPersianDateTime:
                        var time1 = value as PersianDateTime;
                        if (time1 != null)
                        {
                            var v = time1.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, ">=", v);
                        }
                        break;
                    case FilterOperation.LessThanEqualPersianDateTime:
                        var value1 = value as PersianDateTime;
                        if (value1 != null)
                        {
                            var v = value1.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", columnName, "<=", v);
                        }
                        break;*/
                }
            }

            return this;
        }

    }



    /*
    public class FilterQuery<TEntity> where TEntity : IEntity, new()
    {
        public FilterQuery()
        {
            QueryText = null;
        }

        private static string QueryText { get; set; }
        private static string TableName { get; set; }
        private static bool IsDistinct { get; set; }
        private static bool IsOrAggregation { get; set; }
        private bool _create;

        internal string GetQuery()
        {
            var result = "";
            if (string.IsNullOrEmpty(QueryText))
            {
                result = string.Format("SELECT {0} * FROM {1}", IsDistinct ? "DISTINCT" : "", TableName);
            }
            else
            {
                result = string.Format("SELECT {0} * FROM {1} WHERE {2}", IsDistinct ? "DISTINCT" : "", TableName, IsOrAggregation ? QueryText.Replace(")(", ") OR (") : QueryText.Replace(")(", ") AND ("));
            }
            return result;
        }

        //public FilterQuery Create()
        //{
        //    _create = true;
        //    return this;
        //}

        public FilterQuery<TEntity> Distinct
        {
            get
            {
                if (_create) IsDistinct = true;
                return this;
            }
        }

        public FilterQuery<TEntity> Begin
        {
            get
            {
                if (_create) QueryText += "(";
                return this;
            }
        }

        public FilterQuery<TEntity> End
        {
            get
            {
                if (_create) QueryText += ")";
                return this;
            }
        }

        public FilterQuery<TEntity> And
        {
            get
            {
                if (_create) QueryText += " AND ";
                return this;
            }
        }

        public FilterQuery<TEntity> Or
        {
            get
            {
                if (_create) QueryText += " OR ";
                return this;
            }
        }

        public FilterQuery<TEntity> Not
        {
            get
            {

                if (_create) QueryText += " NOT ";
                return this;
            }

        }

        public FilterQuery<TEntity> AggregateByOr
        {
            get
            {

                if (_create) IsOrAggregation = true;
                return this;
            }

        }

        public FilterQuery<TEntity> TableOrView()
        {
            var dtoType = typeof(TEntity);
            var allAttr = dtoType.Attributes().ToList();
            var tableAttribute = allAttr.FirstOrDefault(x => x is TableNameAttribute) as TableNameAttribute;
            var table = tableAttribute == null ? "[" + dtoType.Name + "]" : "[" + tableAttribute.Name + "]";
            var viewAttribute = allAttr.FirstOrDefault(x => x is ViewNameAttribute) as ViewNameAttribute;
            var view = viewAttribute == null ? null : "[" + viewAttribute.Name + "]";

            _create = true;
            if (_create) TableName = view ?? table;
            return this;

        }

        public FilterQuery<TEntity> AddFilter(Expression<Func<TEntity, object>> columnName, FilterOperation filterOperation, object value)
        {
            var cn = columnName.GetMemberName();
            var ema = typeof(TEntity).GetProperty(cn).Attribute<EntityMapAttribute>();
            var colName = ema != null ? ema.ColumnName : cn;

            if (_create)
            {
                switch (filterOperation)
                {
                    case FilterOperation.Equal:
                        QueryText += string.Format("({0} {1} {2})", colName, "=", value);
                        break;
                    case FilterOperation.NotEqual:
                        QueryText += string.Format("({0} {1} {2})", colName, "!=", value);
                        break;
                    case FilterOperation.GreaterThan:
                        QueryText += string.Format("({0} {1} {2})", colName, ">", value);
                        break;
                    case FilterOperation.GreaterThanEqual:
                        QueryText += string.Format("({0} {1} {2})", colName, ">=", value);
                        break;
                    case FilterOperation.LessThan:
                        QueryText += string.Format("({0} {1} {2})", colName, "<", value);
                        break;
                    case FilterOperation.LessThanEqual:
                        QueryText += string.Format("({0} {1} {2})", colName, "<=", value);
                        break;
                    case FilterOperation.Like:
                        QueryText += string.Format("({0} {1} '%{2}%')", colName, "LIKE", value);
                        break;

                    case FilterOperation.MultipleLike:
                        if (value is string[])
                        {
                            var v = "LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.MultipleTotalLike:
                        if (value is string[])
                        {
                            var v = "LIKE '%" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.MultipleStartLike:
                        if (value is string[])
                        {
                            var v = "LIKE '%" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.MultipleEndLike:
                        if (value is string[])
                        {
                            var v = "LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleTotalLike:
                        var strings = value as string[];
                        if (strings != null)
                        {
                            var v = "NOT LIKE '%" + strings.Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleLike:
                        if (value is string[])
                        {
                            var v = "NOT LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleStartLike:
                        if (value is string[])
                        {
                            var v = "NOT LIKE '%" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.NotMultipleEndLike:
                        if (value is string[])
                        {
                            var v = "NOT LIKE '" + ((string[])value).Aggregate((a, b) => a + "%" + b) + "%'";
                            QueryText += string.Format("({0} {1})", colName, v);
                        }
                        break;

                    case FilterOperation.NotLike:
                        QueryText += string.Format("({0} {1} '%{2}%')", colName, "NOT LIKE", value);
                        break;
                    case FilterOperation.StartsWith:
                        QueryText += string.Format("({0} {1} '{2}%')", colName, "LIKE", value);
                        break;
                    case FilterOperation.DoesNotStartWith:
                        QueryText += string.Format("({0} {1} '{2}%')", colName, "NOT LIKE", value);
                        break;
                    case FilterOperation.EndsWith:
                        QueryText += string.Format("({0} {1} '%{2}')", colName, "LIKE", value);
                        break;
                    case FilterOperation.DoesNotEndWith:
                        QueryText += string.Format("({0} {1} '%{2}')", colName, "NOT LIKE", value);
                        break;
                    case FilterOperation.IsNull:
                        QueryText += string.Format("({0} {1})", colName, "IS NULL");
                        break;
                    case FilterOperation.IsNotNull:
                        QueryText += string.Format("({0} {1})", colName, "IS NOT NULL");
                        break;
                    case FilterOperation.In:
                        QueryText += string.Format("({0} {1} ({2}))", colName, "IN", value);
                        break;



                    case FilterOperation.EqualDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, "=", v);
                        }

                        break;
                    case FilterOperation.GreaterThanDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, ">", v);
                        }
                        break;
                    case FilterOperation.LessThanDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, "<", v);
                        }

                        break;
                    case FilterOperation.GreaterThanEqualDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, ">=", v);
                        }
                        break;
                    case FilterOperation.LessThanEqualDateTime:
                        if (value is DateTime)
                        {
                            var v = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, "<=", v);
                        }
                        break;


                    case FilterOperation.EqualPersianDateTime:
                        if (value is PersianDateTime)
                        {
                            var v = ((PersianDateTime)value).ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, "=", v);
                        }
                        break;
                    case FilterOperation.GreaterThanPersianDateTime:
                        if (value is PersianDateTime)
                        {
                            var v = ((PersianDateTime)value).ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, ">", v);
                        }
                        break;
                    case FilterOperation.LessThanPersianDateTime:
                        if (value is PersianDateTime)
                        {
                            var v = ((PersianDateTime)value).ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, "<", v);
                        }
                        break;
                    case FilterOperation.GreaterThanEqualPersianDateTime:
                        if (value is PersianDateTime)
                        {
                            var v = ((PersianDateTime)value).ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, ">=", v);
                        }
                        break;
                    case FilterOperation.LessThanEqualPersianDateTime:
                        if (value is PersianDateTime)
                        {
                            var v = ((PersianDateTime)value).ToDateTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
                            QueryText += string.Format("({0} {1} '{2}')", colName, "<=", v);
                        }
                        break;
                }
            }

            return this;
        }

    }*/
}

