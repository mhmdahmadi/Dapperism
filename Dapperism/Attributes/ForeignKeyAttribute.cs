using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(string relatedPropertyName)
        {
            RelatedPropertyName = relatedPropertyName;
        }
        public string RelatedPropertyName { get; set; }
    }
}