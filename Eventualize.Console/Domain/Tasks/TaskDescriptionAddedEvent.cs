using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Console.Domain
{
    [EventTypeName("TaskDescribed")]
    public class TaskDescriptionAddedEvent : IEventData
    {
        public string Description { get; set; }
    }
}