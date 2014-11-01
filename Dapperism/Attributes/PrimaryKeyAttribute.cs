using System;
using Dapperism.Enums;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(AutoNumber autoNumber = AutoNumber.No)
        {
            AutoNumber = autoNumber;
        }
        public AutoNumber AutoNumber { get; set; }
    }
}