using System.Data;

namespace Dapperism.DataAccess
{
    public class DynamicParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public int? Size { get; set; }
        public ParameterDirection? Direction { get; set; }
        public DbType? DbType { get; set; }
    }
}