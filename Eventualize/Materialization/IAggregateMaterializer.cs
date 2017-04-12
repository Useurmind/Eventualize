using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public class ChosenAggregateTypes
    {
        public ChosenAggregateTypes(IEnumerable<Type> aggregateTypes)
        {
            this.AggregateTypes = aggregateTypes;
        }

        public ChosenAggregateTypes()
        {
            this.AllAggregateTypesChosen = true;
        }

        public bool AllAggregateTypesChosen { get; }

        public IEnumerable<Type> AggregateTypes { get; }
    }

    public interface IAggregateMaterializer
    {
        /// <summary>
        /// Returning null means you want all aggregate events.
        /// </summary>
        ChosenAggregateTypes ChosenAggregateTypes { get; }

        void HandleAggregateEvent(IAggregate aggregate, IAggregateEvent materializationEvent);
    }
}