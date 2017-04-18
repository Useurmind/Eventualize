using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Console.Domain.EventualizeTest.Tasks
{
    [EventTypeName("TaskCreated")]
    public class TaskCreatedEvent : IEventData
    {
        public string Title { get; set; }
    }
}