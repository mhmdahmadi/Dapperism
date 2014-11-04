using System;
using System.Collections.Generic;
using System.Globalization;
using Dapperism.Utilities;

namespace Dapperism.Entities
{
    internal static class EntityAnalyser<TEntity>
         where TEntity : class ,IEntity, new()
    {
        internal static EntityAttributes GetInfo()
        {
            var type = typeof(TEntity);
            var ea = DapperismSettings.GetInfo(type);
            return ea;
        }

        internal static object[] GetValues(TEntity entity)
        {
            var info = GetInfo();
            var props = info.NotSeparated;
            var obj = new object[props.Count];
            int i = 0;

            foreach (var field in props)
            {
                try
                {
                    var value = field.PropertyType == typeof(DateTime) ? ((DateTime)field.GetValue(entity)).ToString(new CultureInfo("en-US")) : field.GetValue(entity).ToString();
                    var arab = DapperismSettings.IsArabicLetters;
                    var pnum = DapperismSettings.IsPersianNums;

                    if (arab)
                        value = value.FixArabicChars();
                    if (pnum)
                        value = value.ToEnglishNumber();
                    obj[i] = "'" + value + "'";
                }
                catch
                {
                }
                i++;
            }
            return obj;
        }

        internal static List<EntityMap> GetData(TEntity entity)
        {
            var info = GetInfo();
            var props = info.NotSeparated;
            var lst = new List<EntityMap>();
            lst.Clear();
            var i = 0;
            foreach (var field in props)
            {
                var key = entity.GetType().FullName.Trim() + "." + field.Name.Trim();
                var entityInfo = info.NotSeparatedInfo[key];
                //var fp = new FastProperty(field);
                //var value = field.PropertyType == typeof(DateTime) ? ((DateTime)fp.Get(entity)).ToString(new CultureInfo("en-US")) : fp.Get(entity).ToString();
                try
                {
                    var value = field.PropertyType == typeof(DateTime) ? ((DateTime)field.GetValue(entity)).ToString(new CultureInfo("en-US")) : field.GetValue(entity).ToString();

                    var arab = DapperismSettings.IsArabicLetters;
                    var pnum = DapperismSettings.IsPersianNums;

                    if (arab)
                        value = value.FixArabicChars();
                    if (pnum)
                        value = value.ToEnglishNumber();

                    lst.Add(new EntityMap()
                    {
                        AutoNumber = entityInfo.AutoNumber,
                        ColumnName = entityInfo.ColumnName,
                        ForeignKeyPropertyName = entityInfo.ForeignKeyPropertyName,
                        IsForeignKey = entityInfo.IsForeignKey,
                        IsSpCudParameter = entityInfo.IsSpCudParameter,
                        SpParamName = entityInfo.SpParamName.Contains("@") ? entityInfo.SpParamName : "@" + entityInfo.SpParamName,
                        IsViewColumn = entityInfo.IsViewColumn,
                        PropertyName = entityInfo.PropertyName,
                        ParameterType = entityInfo.ParameterType,
                        ParameterDirection = entityInfo.ParameterDirection,
                        Value = value,
                        FormattedValue = "'" + value + "'"
                    });
                }
                catch
                {
                }
                i++;
            }
            return lst;
        }
    }
}
