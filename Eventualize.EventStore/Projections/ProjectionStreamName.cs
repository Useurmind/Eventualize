using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.EventStore.Persistence;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.EventStore.Projections
{
    public class ProjectionStreamName
    {
        public ProjectionStreamName(BoundedContextName? boundedContextName=null, AggregateTypeName? aggregateTypeName=null)
        {
            this.BoundedContextName = boundedContextName;
            this.AggregateTypeName = aggregateTypeName;
        }

        public AggregateTypeName? AggregateTypeName { get; }

        public BoundedContextName? BoundedContextName { get; }

        public const string ProjectionPrefix = "Proj-";

        public override string ToString()
        {
            if (this.BoundedContextName.HasValue && this.AggregateTypeName.HasValue)
            {
                return $"{ProjectionPrefix}{AggregateStreamName.AggregatePrefix}{this.BoundedContextName.Value}-{this.AggregateTypeName.Value}";
            }
            else if (this.BoundedContextName.HasValue)
            {
                return $"{ProjectionPrefix}{AggregateStreamName.AggregatePrefix}{this.BoundedContextName.Value}";
            }
            else
            {
                return $"{ProjectionPrefix}{AggregateStreamName.AggregatePrefix}".TrimEnd('-');
            }
        }
    }
}
