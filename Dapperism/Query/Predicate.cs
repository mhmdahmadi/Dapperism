﻿using System;
using System.Linq;
using System.Linq.Expressions;
using Dapperism.Entities;
using Dapperism.Enums;
using Dapperism.Utilities;

namespace Dapperism.Query
{
    public class Predicate
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
            var str = "";
            if (string.IsNullOrEmpty(_qText))
                str = string.Format("SELECT {0} {1} FROM {2}.{3}", _isDistinct ? "DISTINCT" : "", _selectCol, _schemaName, _tableName);
            else
            {
                str = string.Format("SELECT {0} {1} FROM {2}.{3} WHERE {4}", _isDistinct ? "DISTINCT" : "", _selectCol, _schemaName, _tableName, _qText);
            }
            return str;
        }

        public Predicate Select(bool isDistinct = false, params string[] selectClause)
        {
            var select = "*";
            if (selectClause != null && selectClause.Any())
                select = string.Format("[{0}]", selectClause.Aggregate((a, b) => string.Format("{0}] , [{1}", a, b)));
            _selectCol = select;
            _isDistinct = isDistinct;


            return this;
        }


        public Predicate TableOrView(string name)
        {
            _tableName = name;
            return this;
        }

        public Predicate Schema(string name)
        {
            _schemaName = name;
            return this;
        }


        public Predicate Begin
        {
            get
            {
                _qText += " ( ";
                return this;
            }
        }

        public Predicate End
        {
            get
            {
                _qText += " ) ";
                return this;
            }
        }

        public Predicate And(bool forAll = false)
        {
            _allAnd = forAll;
            _allOr = !forAll;
            _qText += " AND ";
            return this;
        }

        public Predicate Or(bool forAll = false)
        {
            _allAnd = !forAll;
            _allOr = forAll;
            _qText += " OR ";
            return this;
        }

        public Predicate Not(bool forAll = false)
        {
            _allOr = forAll;
            _qText += " NOT ";
            return this;
        }

        public Predicate Where<TEntity>(Expression<Func<TEntity, object>> field, FilterOperation filterOperation, object value)
            where TEntity : class, IEntity, new()
        {
            var name = field.GetMemberName();
            var key = typeof(TEntity).FullName.Trim() + "." + name.Trim();
            var data = EntityAnalyser<TEntity>.GetInfo();
            var type = data.NotSeparatedInfo[key].PropertyType;
            var val = Convert.ChangeType(value, type);
            
            switch (filterOperation)
            {
                case FilterOperation.Equal:
                    _qText += string.Format("({0} {1} {2})", name, " = ", value);
                    break;
                case FilterOperation.NotEqual:
                    break;
                case FilterOperation.GreaterThan:
                    break;
                case FilterOperation.GreaterThanEqual:
                    break;
                case FilterOperation.LessThan:
                    break;
                case FilterOperation.LessThanEqual:
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
                    break;
                case FilterOperation.DoesNotStartWith:
                    break;
                case FilterOperation.EndsWith:
                    break;
                case FilterOperation.DoesNotEndWith:
                    break;
                case FilterOperation.IsNull:
                    break;
                case FilterOperation.IsNotNull:
                    break;
                case FilterOperation.In:
                    break;
                case FilterOperation.EqualDateTime:
                    break;
                case FilterOperation.GreaterThanDateTime:
                    break;
                case FilterOperation.LessThanDateTime:
                    break;
                case FilterOperation.GreaterThanEqualDateTime:
                    break;
                case FilterOperation.LessThanEqualDateTime:
                    break;
                case FilterOperation.EqualPersianDateTime:
                    break;
                case FilterOperation.GreaterThanPersianDateTime:
                    break;
                case FilterOperation.LessThanPersianDateTime:
                    break;
                case FilterOperation.GreaterThanEqualPersianDateTime:
                    break;
                case FilterOperation.LessThanEqualPersianDateTime:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("filterOperation");
            }


            return this;
        }


    }
}
