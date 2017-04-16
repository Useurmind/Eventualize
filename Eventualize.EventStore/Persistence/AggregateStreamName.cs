using System;
using System.Linq;
using System.Text.RegularExpressions;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.EventStore.Persistence
{
    public class AggregateStreamName
    {
        public const string AggregatePrefix = "Agg-";

        public AggregateStreamName(BoundedContext boundedContext, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.BoundedContext = boundedContext;
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public BoundedContext BoundedContext { get; }

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
                new BoundedContext(streamNameParts[1]),
                new AggregateTypeName(streamNameParts[2]),
                aggregateId);
        }

        public static AggregateStreamName FromAggregateIdentity(AggregateIdentity aggregateIdentity)
        {
            return new AggregateStreamName(aggregateIdentity.BoundedContext, aggregateIdentity.AggregateTypeName, aggregateIdentity.Id);
        }

        public static bool IsAggregateStreamName(string streamId, BoundedContext eventNameSpace)
        {
            return Regex.IsMatch(streamId, "^" + AggregatePrefix + eventNameSpace.Value + @"-[^\-]*-[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", RegexOptions.IgnoreCase);
        }

        public static bool IsAggregateStreamName(string streamId)
        {
            return Regex.IsMatch(streamId, "^" + AggregatePrefix + @"[^\-]*-[^\-]*-[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", RegexOptions.IgnoreCase);
        }

        public string ToString()
        {
            return $"{AggregatePrefix}{this.BoundedContext.Value}-{this.AggregateTypeName.Value}-{this.AggregateId}";
        }

        public AggregateIdentity GetAggregateIdentity()
        {
            return new AggregateIdentity(this.BoundedContext, this.AggregateTypeName, this.AggregateId);
        }
    }
}