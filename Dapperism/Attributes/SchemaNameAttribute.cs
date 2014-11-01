using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SchemaNameAttribute : Attribute
    {
        public SchemaNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}