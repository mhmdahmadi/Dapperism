using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class RetrieveOnlyAttribute : Attribute
    {
    }
}