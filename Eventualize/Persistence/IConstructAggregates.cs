using System;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;

namespace Eventualize.Persistence
{
    public class AggregateFactory : IConstructAggregates
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            if (snapshot != null)
            {
                throw new NotImplementedException();
            }

            IAggregate aggregate = (IAggregate)Activator.CreateInstance(type, id);
            
            return aggregate;
        }
    }

    public interface IConstructAggregates
    {
        IAggregate Build(Type type, Guid id, IMemento snapshot);
    }

    public static class ConstructAggregatesExtensions
    {
        public static TAggregate NewAggregate<TAggregate>(this IConstructAggregates constructAggregates)
        {
            return (TAggregate)constructAggregates.Build(typeof(TAggregate), Guid.NewGuid(), null);
        }
    }
}