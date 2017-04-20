using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IEventMaterializationActionHandler
    {
        EventMaterializationActionType ActionType { get; }
    }

    public interface IEventMaterializationActionHandler<TEventMaterializationAction> : IEventMaterializationActionHandler
        where TEventMaterializationAction : IEventMaterializationAction
    {
        void Handle(TEventMaterializationAction eventAction, IEvent @event);
    }
}