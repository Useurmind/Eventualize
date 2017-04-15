using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Snapshots;

namespace Eventualize.Console.Domain
{
    public class TaskState : SnapShotBase
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}