using System.Linq;

using Eventualize.Snapshots;

namespace Eventualize.Console.Domain.EventualizeTest.Tasks
{
    public class TaskState : SnapShotBase
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}