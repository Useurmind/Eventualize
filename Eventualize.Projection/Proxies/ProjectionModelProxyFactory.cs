using System;
using System.Collections.Generic;
using System.Linq;

using Castle.DynamicProxy;

namespace Eventualize.Projection.Proxies
{
    /// <summary>
    /// Generate different kinds of proxies.
    /// </summary>
    public static class ProjectionModelProxyFactory
    {
        public static object GenerateProxy(Type projectModelType, IDictionary<string, object> properties)
        {
            var propertyInterceptor = new PropertyStoringInterceptor(properties);

            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(projectModelType, propertyInterceptor);
        }

        public static object GenerateProxy(Type projectModelType)
        {
            var propertyInterceptor = new PropertyStoringInterceptor();

            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(projectModelType, propertyInterceptor);
        }

        public static object GenerateProxy(Type projectModelType, out ProjectionPropertyModificationInterceptor interceptor)
        {
            interceptor = new ProjectionPropertyModificationInterceptor();
            var propertyInterceptor = new PropertyStoringInterceptor();

            return new ProxyGenerator().CreateInterfaceProxyWithoutTarget(projectModelType, interceptor, propertyInterceptor);
        }
    }
}