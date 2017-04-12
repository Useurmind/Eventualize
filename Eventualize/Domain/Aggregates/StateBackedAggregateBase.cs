using System;
using System.Linq;

namespace Eventualize.Domain.Aggregates
{
    public abstract class StateBackedAggregateBase<TState> : AggregateBase
        where TState : class, IMemento, new()
    {
        protected StateBackedAggregateBase()
            : this(Guid.NewGuid())
        {
        }

        protected StateBackedAggregateBase(Guid id)
            : this(new TState() {Id = id, Version = AggregateVersion.NotCreated}, null)
        {
            
        }

        protected StateBackedAggregateBase(TState state, IRouteEvents handler) : base(handler)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            this.State = state;
        }

        protected TState State { get; private set; }

        public override Guid Id
        {
            get
            {
                return this.State.Id;
            }
            protected set
            {
                this.State.Id = value;
            }
        }

        public override long Version
        {
            get
            {
                return this.State.Version;
            }
            protected set
            {
                this.State.Version = value;
            }
        }

        protected override IMemento GetSnapshot()
        {
            return this.State;
        }

        protected override void ApplySnapshot(IMemento snapshot)
        {
            this.State = snapshot as TState;
        }
    }
}