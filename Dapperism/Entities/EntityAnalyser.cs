using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection;
using System.Reflection.Emit;
using Dapperism.Attributes;
using Dapperism.DataAccess;
using Dapperism.Enums;
using Dapperism.Utilities;
using Fasterflect;

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
                var value = "";
                try
                {
                    value = field.PropertyType == typeof(DateTime) ? ((DateTime)field.GetValue(entity)).ToString(new CultureInfo("en-US")) : field.GetValue(entity).ToString();
                }
                catch
                {
                }

                obj[i] = "'" + value + "'";
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
            int i = 0;
            var insertdps = new Dapper.DynamicParameters();
            foreach (var field in props)
            {
                var key = entity.GetType().FullName.Trim() + "." + field.Name.Trim();
                var entityInfo = info.NotSeparatedInfo[key];
                //var fp = new FastProperty(field);
                //var value = field.PropertyType == typeof(DateTime) ? ((DateTime)fp.Get(entity)).ToString(new CultureInfo("en-US")) : fp.Get(entity).ToString();
                var value = "";
                try
                {
                    value = field.PropertyType == typeof(DateTime) ? ((DateTime)field.GetValue(entity)).ToString(new CultureInfo("en-US")) : field.GetValue(entity).ToString();
                }
                catch
                {
                }

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

                i++;
            }
            return lst;
        }
    }
}
