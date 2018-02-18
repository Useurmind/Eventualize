using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

using Castle.DynamicProxy;

namespace Eventualize.Projection.Proxies
{
    /// <summary>
    /// This interceptor remembers all properties that were modified.
    /// </summary>
    public class ProjectionPropertyModificationInterceptor : IInterceptor
    {
        private HashSet<MethodInfo> modifiedPropertieSetters;

        public ProjectionPropertyModificationInterceptor()
        {
            this.modifiedPropertieSetters = new HashSet<MethodInfo>();
        }

        public void Intercept(IInvocation invocation)
        {
            var isGet = invocation.Method.ReturnType != typeof(void);

            if (!isGet)
            {
                this.modifiedPropertieSetters.Add(invocation.Method);
            }

            invocation.Proceed();
        }

        public IEnumerable<PropertyInfo> ModifiedProperties
        {
            get
            {
                return this.modifiedPropertieSetters.Select(x => x.DeclaringType.GetProperty(x.Name.Remove(0, 4)));
            }
        }
    }
}