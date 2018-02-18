using System;
using System.Collections.Generic;
using System.Linq;

using Castle.DynamicProxy;

namespace Eventualize.Projection.Proxies
{
    /// <summary>
    /// This interceptor is the innermost core for all DTO and stores their properties.
    /// </summary>
    public class PropertyStoringInterceptor : IInterceptor
    {
        private IDictionary<string, object> propertyValues;

        public PropertyStoringInterceptor(IDictionary<string, object> properties)
        {
            this.propertyValues = properties;
        }

        public PropertyStoringInterceptor( )
        {
            this.propertyValues = new Dictionary<string, object>();
        }

        public void Intercept(IInvocation invocation)
        {
            var isGet = invocation.Method.ReturnType != typeof(void);
            var propertyName = invocation.Method.Name.Remove(0, 4); // removes either get_ or set_ prefix

            if (!isGet)
            {
                this.propertyValues[propertyName] = invocation.Arguments[0];
            }
            else
            {
                invocation.ReturnValue = this.propertyValues.Keys.Contains(propertyName) ? this.propertyValues[propertyName] : Activator.CreateInstance(invocation.Method.ReturnType);
            }
        }
    }
}