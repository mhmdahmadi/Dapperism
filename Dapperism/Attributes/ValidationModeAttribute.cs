using System;
using Dapperism.Enums;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ValidationModeAttribute : Attribute
    {
        public ValidationModeAttribute(CascadeMode cascadeMode = CascadeMode.Continue)
        {
            CascadeMode = cascadeMode;
        }
        public CascadeMode CascadeMode { get; set; }
    }
}