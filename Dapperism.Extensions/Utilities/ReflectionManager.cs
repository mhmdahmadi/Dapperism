using System;

namespace Dapperism.Extensions.Utilities
{
    public sealed class ReflectionManager
    {
        public static TProperty GetPropertyValue<TClass, TProperty>(TClass instance, string propertyName) where TClass : class
        {
            if (propertyName == null || string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName", "Value can not be null or empty.");

            object obj = null;
            var type = instance.GetType();
            var info = type.GetProperty(propertyName);
            if (info != null)
                obj = info.GetValue(instance, null);
            return (TProperty)obj;
        }

        public static void SetPropertyValue<TClass>(TClass classInstance, string propertyName, object propertyValue) where TClass : class
        {
            if (propertyName == null || string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("propertyName", "Value can not be null or empty.");

            var type = classInstance.GetType();
            var info = type.GetProperty(propertyName);

            if (info != null)
                info.SetValue(classInstance, Convert.ChangeType(propertyValue, info.PropertyType), null);
        }
    }
}
