using System;
using System.Collections.Generic;
using System.Reflection;
using Dapperism.Enums;

namespace Dapperism.Entities
{
    internal class EntityAttributes
    {
        internal Type EntityType { get; set; }
        internal string SchemaName { get; set; }
        internal string TableName { get; set; }
        internal string ViewName { get; set; }
        internal string InsertSpName { get; set; }
        internal string UpdateSpName { get; set; }
        internal string DeleteSpName { get; set; }
        internal string SelectByIdSpName { get; set; }
        internal string SelectAllSpName { get; set; }
        internal bool RetrieveOnly { get; set; }
        internal Dictionary<string, EntityInfo> NotSeparatedInfo { get; set; }
        internal IList<PropertyInfo> NotSeparated { get; set; }
        internal string[] PropertyNames { get; set; }
        internal CascadeMode CascadeMode { get; set; }
    }
}
