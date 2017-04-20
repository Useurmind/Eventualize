using System.Linq;

using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class FluentMaterializationContext
    {
        private MaterializationFactory materializationFactory;

        private IEventualizeContainerBuilder containerBuilder;

        public FluentMaterializationContext(MaterializationFactory materializationFactory, IEventualizeContainerBuilder containerBuilder )
        {
            this.materializationFactory = materializationFactory;
            this.containerBuilder = containerBuilder;
        }

        public void RegisterEventMaterializationAction(IEventMaterializationAction eventMaterializationAction)
        {
            this.containerBuilder.RegisterSingleInstance(c => eventMaterializationAction);
        }

        public IMaterializationFactory GetFactory()
        {
            return this.materializationFactory;
        }
    }
}