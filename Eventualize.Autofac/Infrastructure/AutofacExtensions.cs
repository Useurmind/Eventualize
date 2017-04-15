using System;
using System.Linq;

using Autofac;

using Eventualize.Interfaces.Infrastructure;

namespace Eventualize.Autofac.Infrastructure
{
    public static class AutofacExtensions
    {
        public static IEventualizeContainer Eventualize(this IContainer container)
        {
            return new AutofacContainer(container);
        }

        public static IEventualizeContainer Eventualize(this IComponentContext componentContext)
        {
            return new AutofacComponentContext(componentContext);
        }

        public static IEventualizeContainerBuilder Eventualize(this ContainerBuilder containerBuilder)
        {
            return new AutofacContainerBuilder(containerBuilder);
        }

        public static ContainerBuilder Eventualize(this ContainerBuilder containerBuilder, Action<IEventualizeContainerBuilder> registerEventualize)
        {
            registerEventualize(new AutofacContainerBuilder(containerBuilder));
            return containerBuilder;
        }
    }
}