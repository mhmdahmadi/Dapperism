﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using Dapperism.Attributes;
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
            var typeFullName = type.FullName;

            if (CacheManager.Instance.Contains(typeFullName))
                return CacheManager.Instance[typeFullName] as EntityAttributes;

            var allAttr = type.Attributes().ToList();

            var schemaAttribute = allAttr.FirstOrDefault(x => x is SchemaNameAttribute) as SchemaNameAttribute;
            var schemaName = schemaAttribute == null ? "[dbo]" : "[" + schemaAttribute.Name.Trim() + "]";

            var tableAttribute = allAttr.FirstOrDefault(x => x is TableNameAttribute) as TableNameAttribute;
            var tableName = tableAttribute == null ? "[" + type.Name.Trim() + "]" : "[" + tableAttribute.Name.Trim() + "]";

            var viewAttribute = allAttr.FirstOrDefault(x => x is ViewNameAttribute) as ViewNameAttribute;
            var viewName = viewAttribute == null ? null : "[" + viewAttribute.Name.Trim() + "]";

            var insSpAttribute = allAttr.FirstOrDefault(x => x is InsertSpNameAttribute) as InsertSpNameAttribute;
            var insSpName = insSpAttribute == null ? null : insSpAttribute.Name.Trim();

            var updSpAttribute = allAttr.FirstOrDefault(x => x is UpdateSpNameAttribute) as UpdateSpNameAttribute;
            var updSpName = updSpAttribute == null ? null : updSpAttribute.Name.Trim();

            var delSpAttribute = allAttr.FirstOrDefault(x => x is DeleteSpNameAttribute) as DeleteSpNameAttribute;
            var delSpName = delSpAttribute == null ? null : delSpAttribute.Name.Trim();

            var sAllAttribute = allAttr.FirstOrDefault(x => x is SelectAllSpNameAttribute) as SelectAllSpNameAttribute;
            var sAllName = sAllAttribute == null ? null : sAllAttribute.Name.Trim();

            var sByIdAttribute = allAttr.FirstOrDefault(x => x is SelectByIdSpNameAttribute) as SelectByIdSpNameAttribute;
            var sByIdName = sByIdAttribute == null ? null : sByIdAttribute.Name.Trim();


            var rOnlyAttribute = allAttr.FirstOrDefault(x => x is RetrieveOnlyAttribute) as RetrieveOnlyAttribute;
            var rOnly = rOnlyAttribute != null;

            var props = type.Properties(Flags.Instance | Flags.DeclaredOnly | Flags.Public);
            var notSeparated = props.Where(x => x.Attribute<SeparatedAttribute>() == null).ToList();

            var propNames = notSeparated.Select(x => x.Name).ToArray();

            var dic = new Dictionary<string, EntityInfo>();
            foreach (var p in notSeparated)
            {
                var key = type.FullName.Trim() + "." + p.Name.Trim();

                var spParamName = p.GetCustomAttribute<SpParameterNameAttribute>();
                var colName = p.GetCustomAttribute<ColumnNameAttribute>();
                var isViewCol = p.GetCustomAttribute<ViewColumnAttribute>();
                var paramDirection = p.GetCustomAttribute<ParameterDirectionAttribute>();
                var isNotSpCud = p.GetCustomAttribute<NotSpParameterAttribute>();
                var fKey = p.GetCustomAttribute<ForeignKeyAttribute>();
                var pKey = p.GetCustomAttribute<PrimaryKeyAttribute>();

                var pk = -1;
                if (pKey != null)
                {
                    switch (pKey.AutoNumber)
                    {
                        case AutoNumber.Yes:
                            pk = 1;
                            break;
                        case AutoNumber.No:
                            pk = 0;
                            break;
                    }
                }

                dic.Add(key, new EntityInfo()
                {
                    AutoNumber = pk,
                    ColumnName = colName == null ? p.Name.Trim() : colName.Name.Trim(),
                    SpParamName = spParamName == null ? p.Name.Trim() : spParamName.Name.Trim(),
                    IsViewColumn = isViewCol != null,
                    ParameterDirection = paramDirection == null ? pk == 1 ? ParameterDirection.InputOutput : ParameterDirection.Input : paramDirection.ParamDirection,
                    ParameterType = p.PropertyType.ToDbType(),
                    IsForeignKey = fKey != null,
                    IsSpCudParameter = isNotSpCud == null,
                    PropertyName = p.Name.Trim(),
                    PropertyType = p.PropertyType,
                    ForeignKeyPropertyName = fKey == null ? null : fKey.RelatedPropertyName.Trim()
                });

            }

            var anc = dic.Values.Count(x => x.AutoNumber == 1);
            if (anc > 1)
                throw new Exception("Each entity can only have an AutoNumber.Yes property");

            var an = dic.Values.Count(x => x.AutoNumber == 1 || x.AutoNumber == 0);
            if (an == 0)
                throw new Exception("Each entity must have a PrimaryKey attribute at least");


            var cols = dic.Values.Select(x => x.ColumnName).ToList();
            var dicNum = new Dictionary<string, string>();
            for (int i = 0; i < cols.Count(); i++)
            {
                dicNum.Add(cols[i], "{" + i + "}");
            }


            var haveAutoNum = anc == 1;

            var cMode = CascadeMode.Continue;
            var vMode = allAttr.FirstOrDefault(x => x is ValidationModeAttribute) as ValidationModeAttribute;
            if (vMode != null)
                cMode = vMode.CascadeMode;

            var result =
                dic.Where(x => x.Value.AutoNumber != -1 && !x.Value.IsViewColumn)
                    .OrderBy(x => x.Value.ColumnName).ToList();

            var lstPk = result.Select(r => new Tuple<string, string, DbType>(r.Value.ColumnName, r.Value.SpParamName, r.Value.ParameterType)).ToList();

            var stv = string.Format("{0}.{1}", schemaName, (string.IsNullOrEmpty(viewName) ? tableName : viewName));

            var whereSt = lstPk.Select(x => x.Item1)
                .Aggregate("", (current, ws) => current + string.Format("[{0}] = {1}", ws, dicNum[ws])).Replace("}[", "} , [");

            var fieldsUpdate =
                dic.Where(x => x.Value.AutoNumber == -1 && !x.Value.IsViewColumn).Select(x => x.Value.ColumnName);

            var setStr =
                fieldsUpdate.Aggregate("",
                    (current, colName) => current + ("[" + colName + "] = " + dicNum[colName]))
            .Replace("}[", "} , [");

            var upSt = string.Format("UPDATE {0}.{1} SET {2} WHERE {3}", schemaName, tableName, setStr, whereSt);

            var srSt = string.Format("SELECT * FROM {0}", stv);

            var srIdSt = string.Format("SELECT * FROM {0} WHERE {1}", stv, whereSt);

            var fIns = dic.Where(x => x.Value.AutoNumber != (int)AutoNumber.Yes && !x.Value.IsViewColumn)
                .Select(x => x.Value.ColumnName).ToList();

            var fieldsInsert = string.Format("[{0}]", fIns.Aggregate((a, b) => a + "] , [" + b));

            var valuesInsert = fIns.Aggregate("", (current, g) => current + dicNum[g]).Replace("}{", "} , {");

            var insStr = string.Format("INSERT INTO {0}.{1} ({2}) VALUES ({3}) ", schemaName,
                tableName, fieldsInsert, valuesInsert);

            string insRetStr;
            var pka = dic.FirstOrDefault(x => x.Value.AutoNumber == (int)AutoNumber.Yes && !x.Value.IsViewColumn).Value;
            if (pka != null)
            {
                var pkCol = pka.ColumnName;
                var iRetStr = string.Format("SELECT * FROM {0}.{1} WHERE {2} = SCOPE_IDENTITY() ",
                    schemaName, tableName, pkCol);
                insRetStr = insStr + iRetStr;
            }
            else
                insRetStr = "";

            var delSt = string.Format("DELETE FROM {0}.{1} WHERE {2}", schemaName, tableName, whereSt);

            var ea = new EntityAttributes
            {
                EntityType = type,
                UpdateSpName = updSpName,
                DeleteSpName = delSpName,
                InsertSpName = insSpName,
                SchemaName = schemaName,
                TableName = tableName,
                ViewName = viewName,
                NotSeparatedInfo = dic,
                NotSeparated = notSeparated,
                PropertyNames = propNames,
                CascadeMode = cMode,
                SelectAllSpName = sAllName,
                SelectByIdSpName = sByIdName,
                RetrieveOnly = rOnly,
                PrimaryKeys = lstPk,
                WhereStatement = whereSt,
                STVCombination = stv,
                UpdateStatement = upSt,
                SelectAllStatement = srSt,
                SelectByIdStatement = srIdSt,
                InsertStatement = insStr,
                InsertReturnStatement = insRetStr,
                DeleteStatement = delSt,
                HaveAutoNumber = haveAutoNum
            };


            CacheManager.Instance[typeFullName] = ea;
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
