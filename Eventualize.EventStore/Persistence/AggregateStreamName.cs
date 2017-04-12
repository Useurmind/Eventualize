using System;
using System.Linq;
using System.Text.RegularExpressions;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Core;

namespace Eventualize.EventStore.Persistence
{
    public class AggregateStreamName
    {
        public const string AggregatePrefix = "Agg-";

        public AggregateStreamName(EventNamespace eventNamespace, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.EventNamespace = eventNamespace;
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public EventNamespace EventNamespace { get; }

        public Guid AggregateId { get; }

        public AggregateTypeName AggregateTypeName { get; }

        public static AggregateStreamName FromStreamName(string streamName)
        {
            var streamNameParts = streamName.Split('-');
            var guidPart = string.Join("-", streamNameParts.Skip(3));
            Guid aggregateId;

            if (streamNameParts.Length < 4 || !Guid.TryParse(guidPart, out aggregateId))
            {
                throw new Exception($"Stream name {streamName} is not valid. A stream name must have the form Agg-<Namespace>-<AggregateTypeName>-<AggregateGuid>");
            }

            return new AggregateStreamName(
                new EventNamespace(streamNameParts[1]),
                new AggregateTypeName(streamNameParts[2]),
                aggregateId);
        }

        public static AggregateStreamName FromAggregateType(Type aggregateType, Guid id, EventNamespace eventNameSpace)
        {
            return new AggregateStreamName(eventNameSpace, aggregateType.GetAggregtateTypeName(), id);
        }

        public static AggregateStreamName FromAggregateIdentity(AggregateIdentity aggregateIdentity)
        {
            return new AggregateStreamName(aggregateIdentity.EventSpace, aggregateIdentity.AggregateTypeName, aggregateIdentity.Id);
        }

        public static bool IsAggregateStreamName(string streamId, EventNamespace eventNameSpace)
        {
            return Regex.IsMatch(streamId, "^" + AggregatePrefix + eventNameSpace.Value + @"-[^\-]*-[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", RegexOptions.IgnoreCase);
        }

        public string ToString()
        {
            return $"{AggregatePrefix}{this.EventNamespace.Value}-{this.AggregateTypeName.Value}-{this.AggregateId}";
        }

        public AggregateIdentity GetAggregateIdentity()
        {
            return new AggregateIdentity()
            {
                Id = this.AggregateId,
                EventSpace = this.EventNamespace,
                AggregateTypeName = this.AggregateTypeName
            };
        }
    }
}