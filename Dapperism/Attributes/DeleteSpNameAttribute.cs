using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DeleteSpNameAttribute : Attribute
    {
        public DeleteSpNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}