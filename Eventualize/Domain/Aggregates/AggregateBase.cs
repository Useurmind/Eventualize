using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain.Aggregates.EventRouting;
using Eventualize.Interfaces;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.Snapshots;
using Eventualize.Snapshots;

namespace Eventualize.Domain.Aggregates
{
    public abstract class AggregateBase : IAggregate, IEquatable<IAggregate>
    {
        private readonly ICollection<object> uncommittedEvents = new LinkedList<object>();

        private IRouteEvents registeredRoutes;

        protected AggregateBase()
            : this(null)
        { }

        protected AggregateBase(IRouteEvents handler)
        {
            if (handler != null)
            {
                this.RegisteredRoutes = handler;
                this.RegisteredRoutes.Register(this);
            }
        }

        protected IRouteEvents RegisteredRoutes
        {
            get
            {
                return this.registeredRoutes ?? (this.registeredRoutes = new ConventionEventRouter(true, this));
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("AggregateBase must have an event router to function");
                }

                this.registeredRoutes = value;
            }
        }
        public abstract Guid Id { get; protected set; }

        public abstract long Version { get; protected set; }

        public long CommittedVersion
        {
            get
            {
                return this.Version - this.uncommittedEvents.Count;
            }
        }
        

        /// <summary>
        /// Called when loading an event with already commited events.
        /// </summary>
        /// <param name="event"></param>
        void IAggregate.ApplyEvent(object @event)
        {
            this.RegisteredRoutes.Dispatch(@event);
            this.Version++;
        }

        ICollection IAggregate.GetUncommittedEvents()
        {
            return (ICollection)this.uncommittedEvents;
        }

        void IAggregate.ClearUncommittedEvents()
        {
            this.uncommittedEvents.Clear();
        }

        ISnapShot IAggregate.GetSnapshot()
        {
            ISnapShot snapshot = this.GetSnapshot();
            snapshot.Id = this.Id;
            snapshot.Version = this.Version;
            return snapshot;
        }

        void IAggregate.ApplySnapshot(ISnapShot snapshot)
        {
            this.ApplySnapshot(snapshot);
        }

        public virtual bool Equals(IAggregate other)
        {
            return null != other && other.Id == this.Id;
        }

        protected void Register<T>(Action<T> route)
        {
            this.RegisteredRoutes.Register(route);
        }

        /// <summary>
        /// Called when a new event is applied that changes the aggregate and needs to be commited.
        /// </summary>
        /// <param name="event"></param>
        protected void RaiseEvent(object @event)
        {
            ((IAggregate)this).ApplyEvent(@event);
            this.uncommittedEvents.Add(@event);
        }

        protected virtual ISnapShot GetSnapshot()
        {
            return null;
        }
        protected virtual void ApplySnapshot(ISnapShot snapshot)
        {
            
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as IAggregate);
        }
    }
}

