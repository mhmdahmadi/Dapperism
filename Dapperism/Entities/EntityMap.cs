using System.Data;


namespace Dapperism.Entities
{
    internal class EntityMap
    {     
        internal string ColumnName { get; set; }
        internal string SpParamName { get; set; }
        internal ParameterDirection ParameterDirection { get; set; }
        internal int AutoNumber { get; set; }
        internal bool IsViewColumn { get; set; }
        internal bool IsSpCudParameter { get; set; }
        internal string Value { get; set; }
        internal string FormattedValue { get; set; }
        internal string PropertyName { get; set; }
        internal DbType ParameterType { get; set; }
        internal string ForeignKeyPropertyName { get; set; }
        internal bool IsForeignKey { get; set; }
    }
}
