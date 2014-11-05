using System;
using System.Data;

namespace Dapperism.EntityAnalysis
{
    internal class EntityInfo
    {
        internal string ColumnName { get; set; }
        internal string SpParamName { get; set; }
        internal ParameterDirection ParameterDirection { get; set; }
        internal int AutoNumber { get; set; }
        internal bool IsViewColumn { get; set; }
        internal bool IsSpCudParameter { get; set; }
        internal string PropertyName { get; set; }
        internal Type PropertyType { get; set; }
        internal DbType ParameterType { get; set; }
        internal bool IsForeignKey { get; set; }
        internal string ForeignKeyPropertyName { get; set; }
    }
}