using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SpParameterNameAttribute : Attribute
    {
        public SpParameterNameAttribute(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }
}