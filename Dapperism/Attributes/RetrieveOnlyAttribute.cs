using System;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class RetrieveOnlyAttribute : Attribute
    {
    }
}