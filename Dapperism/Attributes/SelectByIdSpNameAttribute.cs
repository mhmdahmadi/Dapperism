using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SelectByIdSpNameAttribute : Attribute
    {
        public SelectByIdSpNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}