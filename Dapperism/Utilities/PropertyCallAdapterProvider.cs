using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dapperism.Utilities
{
    public interface IPropertyCallAdapter<in TThis>
    {
        object InvokeGet(TThis @this);
        //add void InvokeSet(TThis @this, object value) if necessary
    }

    public class PropertyCallAdapter<TThis, TResult> : IPropertyCallAdapter<TThis>
    {
        private readonly Func<TThis, TResult> _getterInvocation;

        public PropertyCallAdapter(Func<TThis, TResult> getterInvocation)
        {
            _getterInvocation = getterInvocation;
        }

        public object InvokeGet(TThis @this)
        {
            return _getterInvocation.Invoke(@this);
        }
    }

    public class PropertyCallAdapterProvider<TThis>
    {
        private static readonly Dictionary<string, IPropertyCallAdapter<TThis>> Instances =
            new Dictionary<string, IPropertyCallAdapter<TThis>>();

        public static IPropertyCallAdapter<TThis> GetInstance(PropertyInfo pi, string forPropertyName)
        {
            IPropertyCallAdapter<TThis> instance;
            if (!Instances.TryGetValue(forPropertyName, out instance))
            {
                var property = pi;

                MethodInfo getMethod;
                Delegate getterInvocation = null;
                if (property != null && (getMethod = property.GetGetMethod(true)) != null)
                {
                    var openGetterType = typeof(Func<,>);
                    var concreteGetterType = openGetterType
                        .MakeGenericType(typeof(TThis), property.PropertyType);

                    getterInvocation =
                        Delegate.CreateDelegate(concreteGetterType, null, getMethod);
                }
                else
                {
                    //throw exception or create a default getterInvocation returning null
                }

                var openAdapterType = typeof(PropertyCallAdapter<,>);
                if (property != null)
                {
                    var concreteAdapterType = openAdapterType
                        .MakeGenericType(typeof(TThis), property.PropertyType);
                    instance = Activator
                        .CreateInstance(concreteAdapterType, getterInvocation)
                        as IPropertyCallAdapter<TThis>;
                }

                Instances.Add(forPropertyName, instance);
            }

            return instance;
        }
    }
}
