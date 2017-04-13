using System;
using System.Collections.Generic;
using System.Linq;

namespace Eventualize.Materialization.AggregateMaterialization
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
}