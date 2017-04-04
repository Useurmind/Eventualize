using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventualize.Domain
{
    public struct AggregateIdentity
    {
        public Guid Id;

        public string AggregateTypeName;
    }
}
