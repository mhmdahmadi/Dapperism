using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SelectAllSpNameAttribute : Attribute
    {
        public SelectAllSpNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}