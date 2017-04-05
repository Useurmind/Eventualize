using System;
using System.Linq;
using System.Text.RegularExpressions;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.EventStore.Persistence
{
    public class StreamName
    {
        public const string AggregatePrefix = "Agg-";

        public StreamName(string aggregateTypeName, Guid aggregateId)
        {
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }

        public string AggregateTypeName { get; }
        
        public static StreamName FromStreamName(string streamName)
        {
            var streamNameParts = streamName.Split('-');
            var guidPart = string.Join("-", streamNameParts.Skip(2));
            Guid aggregateId;

            if (streamNameParts.Length < 3 || !Guid.TryParse(guidPart, out aggregateId))
            {
                throw new Exception($"Stream name {streamName} is not valid. A stream name must have the form <BucketId>-<AggregateTypeName>-<AggregateGuid>");
            }

            return new StreamName(streamNameParts[1], aggregateId);
        }

        public static StreamName FromAggregateType(Type aggregateType, Guid id)
        {
            return new StreamName(aggregateType.GetAggregtateTypeName(), id);
        }

        public static bool IsAggregateStreamName(string streamId)
        {
            return Regex.IsMatch(streamId, "^" + AggregatePrefix + @"[^\-]*-[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", RegexOptions.IgnoreCase);
        }

        public string ToString()
        {
            return $"{AggregatePrefix}{this.AggregateTypeName}-{this.AggregateId}";
        }

        public AggregateIdentity GetAggregateIdentity()
        {
            return new AggregateIdentity()
                   {
                       Id = this.AggregateId,
                       AggregateTypeName = this.AggregateTypeName
                   };
        }
    }
}