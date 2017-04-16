using System;
using System.Linq;

namespace Eventualize.Domain.Events
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class BoundedContextAttribute : Attribute
    {
        public BoundedContextAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }
}