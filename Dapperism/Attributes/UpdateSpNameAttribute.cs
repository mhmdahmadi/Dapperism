using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class UpdateSpNameAttribute : Attribute
    {
        public UpdateSpNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}