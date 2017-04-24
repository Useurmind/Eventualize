using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams.EventController
{
    public interface IEventControllerContext
    {
        IEvent Event { get; }
    }

    public interface IHandleEvent<T>
        where T : IEventData
    {
        void Handle(T eventData);
    }

    public class EventController
    {
        public EventController(IEventControllerContext context)
        {
            this.Context = context;
        }

        protected IEventControllerContext Context { get; }
    }
}
