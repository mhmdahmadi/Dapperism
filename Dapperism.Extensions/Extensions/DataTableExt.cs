using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Dapperism.Extensions.Extensions
{
    public static class DataTableExt
    {
        public static DataTable ToDataTable(this IDataReader dr)
        {
            var result = new DataTable();
            result.Load(dr);
            return result;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        public static DataTable ToDataTable(this IEnumerable<dynamic> items)
        {
            var data = items.ToArray();
            if (!data.Any()) return null;

            var dt = new DataTable();
            foreach (var key in ((IDictionary<string, object>)data[0]).Keys)
            {
                dt.Columns.Add(key);
            }
            foreach (var d in data)
            {
                dt.Rows.Add(((IDictionary<string, object>)d).Values.ToArray());
            }
            return dt;
        }
     
        public static DataRow GetEntireRow(this IDataReader reader)
        {
            var table = new DataTable();
            var count = reader.FieldCount;
            var colValues = new string[count];
            var types = new Type[count];

            for (var i = 0; i < count; i++)
            {
                colValues[i] = reader[i].ToString();
                types[i] = reader.GetFieldType(i);
                var colName = reader.GetName(i);
                table.Columns.Add(colName, types[i]);
            }

            var obj = new object[count];
            for (var i = 0; i < count; i++)
                obj[i] = Convert.ChangeType(colValues[i], types[i]);

            table.Rows.Add(obj);
            return table.Rows[0];
        }

        public static TReturn SafeColumnReader<TEntity, TReturn>(this IDataReader reader, Expression<Func<TEntity, object>> columnName)
                where TEntity : class,new()
        {
            return reader.SafeColumnReader<TReturn>(columnName.GetMemberName().ToUpperInvariant());
        }

        public static TReturn SafeColumnReader<TReturn>(this IDataReader reader, string columnName)
        {
            try
            {
                object retValue = null;
                var type = typeof(TReturn);
                var index = reader.GetOrdinal(columnName);
                if (!reader.IsDBNull(index))
                {
                    if (type == typeof(bool))
                        retValue = reader.GetBoolean(index);

                    if (type == typeof(byte))
                        retValue = reader.GetByte(index);

                    if (type == typeof(byte[]))
                    {
                        var byteArray = new byte[(reader.GetBytes(index, 0, null, 0, int.MaxValue))];
                        reader.GetBytes(index, 0, byteArray, 0, byteArray.Length);
                        retValue = byteArray;
                    }

                    if (type == typeof(char))
                        retValue = reader.GetChar(index);

                    if (type == typeof(char[]))
                    {
                        var charArray = new char[(reader.GetChars(index, 0, null, 0, int.MaxValue))];
                        reader.GetChars(index, 0, charArray, 0, charArray.Length);
                        retValue = charArray;
                    }

                    if (type == typeof(IDataReader))
                        retValue = reader.GetData(index);

                    if (type == typeof(DateTime))
                        retValue = reader.GetDateTime(index);

                    if (type == typeof(decimal))
                        retValue = reader.GetDecimal(index);

                    if (type == typeof(double))
                        retValue = reader.GetDouble(index);

                    if (type == typeof(Type))
                        retValue = reader.GetFieldType(index);

                    if (type == typeof(float))
                        retValue = reader.GetFloat(index);

                    if (type == typeof(Guid))
                        retValue = reader.GetGuid(index);

                    if (type == typeof(short))
                        retValue = reader.GetInt16(index);

                    if (type == typeof(int))
                        retValue = reader.GetInt32(index);

                    if (type == typeof(long))
                        retValue = reader.GetInt64(index);

                    if (type == typeof(object))
                        retValue = reader.GetValue(index);

                    if (type == typeof(string))
                        retValue = reader.GetString(index);

                    return (TReturn)retValue;
                }
                return default(TReturn);
            }
            catch (Exception ex)
            {
                if (ex is IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException("The column name does not exist.");
                }

                if (ex is InvalidCastException)
                {
                    throw new InvalidCastException("The return type, with the type of data in the database is not in compliance");
                }
                return default(TReturn);
            }
        }
    }
}
