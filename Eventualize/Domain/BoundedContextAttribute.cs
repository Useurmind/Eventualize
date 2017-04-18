using System;
using System.Linq;

namespace Eventualize.Domain.Events
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class BoundedContextAttribute : Attribute
    {
        /// <summary>
        /// This constructor extracts the bounded context name from the namespace of the aggregate.
        /// The second single namespace name above the actual type is used as context name.
        /// The namespace MyAssembly.Something.MyContext.MyAggregates.MyAggregate would lead to bounded context name MyContext.
        /// </summary>
        public BoundedContextAttribute( )
        {
        }

        public BoundedContextAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}