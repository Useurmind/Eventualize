using System;
using System.Linq;
using System.Text.RegularExpressions;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.EventStore.Persistence
{
    public class AggregateStreamName
    {
        public const string AggregatePrefix = "Agg-";

        public AggregateStreamName(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.BoundedContextName = boundedContextName;
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public BoundedContextName BoundedContextName { get; }

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
                new BoundedContextName(streamNameParts[1]),
                new AggregateTypeName(streamNameParts[2]),
                aggregateId);
        }

        public static AggregateStreamName FromAggregateIdentity(AggregateIdentity aggregateIdentity)
        {
            return new AggregateStreamName(aggregateIdentity.BoundedContextName, aggregateIdentity.AggregateTypeName, aggregateIdentity.Id);
        }

        public static bool IsAggregateStreamName(string streamId, BoundedContextName boundedContextName)
        {
            return Regex.IsMatch(streamId, "^" + GetBoundedContextPrefix(boundedContextName) + @"[^\-]*-[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", RegexOptions.IgnoreCase);
        }

        public static bool IsAggregateStreamName(string streamId)
        {
            return Regex.IsMatch(streamId, "^" + AggregatePrefix + @"[^\-]*-[^\-]*-[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", RegexOptions.IgnoreCase);
        }

        public static string GetBoundedContextPrefix(BoundedContextName boundedContextName)
        {
            return $"{AggregatePrefix}{boundedContextName.Value}-";
        }

        public static string GetAggregateTypePrefix(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName)
        {
            return $"{GetBoundedContextPrefix(boundedContextName)}{aggregateTypeName.Value}-";
        }

        public string ToString()
        {
            return $"{GetAggregateTypePrefix(this.BoundedContextName, this.AggregateTypeName)}{this.AggregateId}";
        }

        public AggregateIdentity GetAggregateIdentity()
        {
            return new AggregateIdentity(this.BoundedContextName, this.AggregateTypeName, this.AggregateId);
        }
    }
}