using System;
using System.Data;

namespace Dapperism.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ParameterDirectionAttribute : Attribute
    {
        public ParameterDirectionAttribute(ParameterDirection parameterDirection = ParameterDirection.Input)
        {
            ParamDirection = parameterDirection;
        }
        public ParameterDirection ParamDirection { get; set; }
    }
}