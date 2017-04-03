using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Console.Domain
{
    [EventTypeName("TaskCreated")]
    public class TaskCreatedEvent : IEventData
    {
        public string Title { get; set; }
    }
}