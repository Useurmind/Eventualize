using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

using Castle.DynamicProxy;

namespace Eventualize.Dapper.Materialization
{
    /// <summary>
    /// This interceptor remembers all properties that were modified.
    /// </summary>
    public class ProjectionPropertyModificationInterceptor : IInterceptor
    {
        private IDictionary<string, object> propertyValues;

        private HashSet<MethodInfo> modifiedPropertieSetters;

        public ProjectionPropertyModificationInterceptor()
        {
            this.propertyValues = new Dictionary<string, object>();
            this.modifiedPropertieSetters = new HashSet<MethodInfo>();
        }

        public void Intercept(IInvocation invocation)
        {
            var isGet = invocation.Method.ReturnType != typeof(void);
            var propertyName = invocation.Method.Name.Remove(0, 4); // removes either get_ or set_ prefix

            if (!isGet)
            {
                this.modifiedPropertieSetters.Add(invocation.Method);
                this.propertyValues[propertyName] = invocation.Arguments[0];
            }
            else
            {
                invocation.ReturnValue = this.propertyValues.Keys.Contains(propertyName) ? this.propertyValues[propertyName] : Activator.CreateInstance(invocation.Method.ReturnType);
            }
        }

        public IEnumerable<PropertyInfo> ModifiedProperties
        {
            get
            {
                return this.modifiedPropertieSetters.Select(x => x.DeclaringType.GetProperty(x.Name.Remove(0, 4)));
            }
        }
    }

    /// <summary>
    /// Generate different kinds of proxies.
    /// </summary>
    public static class ProjectionModelProxyFactory
    {
        public static object GenerateProxy(Type projectModelType, out ProjectionPropertyModificationInterceptor interceptor)
        {
            interceptor = new ProjectionPropertyModificationInterceptor();
            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(projectModelType, interceptor);
        }
    }
}