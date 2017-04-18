using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Eventualize.Interfaces.Persistence
{
    /// <summary>
    /// This exception should be thrown when the expected aggregate version is not encountered when appending events.
    /// </summary>
    [Serializable]
    public class ExpectedAggregateVersionException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ExpectedAggregateVersionException()
        {
        }

        public ExpectedAggregateVersionException(string message)
            : base(message)
        {
        }

        public ExpectedAggregateVersionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ExpectedAggregateVersionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
