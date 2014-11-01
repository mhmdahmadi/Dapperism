using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ViewNameAttribute : Attribute
    {
        public ViewNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}