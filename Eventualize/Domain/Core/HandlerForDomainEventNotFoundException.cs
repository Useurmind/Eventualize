using System;
using System.Linq;

namespace Eventualize.Domain.Core
{
    public class HandlerForDomainEventNotFoundException : Exception
    {
        public HandlerForDomainEventNotFoundException()
        { }

        public HandlerForDomainEventNotFoundException(string message)
            : base(message)
        { }

        public HandlerForDomainEventNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}