using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class InsertSpNameAttribute : Attribute
    {
        public InsertSpNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}