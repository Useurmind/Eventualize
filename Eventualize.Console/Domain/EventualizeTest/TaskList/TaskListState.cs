using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Snapshots;

namespace Eventualize.Console.Domain.EventualizeTest.TaskList
{
    public class TaskListState : SnapShotBase
    {
        public string Name { get; set; }

        public IEnumerable<Guid> Tasks { get; set; }
    }
}