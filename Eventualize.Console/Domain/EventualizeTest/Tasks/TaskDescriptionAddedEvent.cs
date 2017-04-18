using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Console.Domain.EventualizeTest.Tasks
{
    [EventTypeName("TaskDescribed")]
    public class TaskDescriptionAddedEvent : IEventData
    {
        public string Description { get; set; }
    }
}