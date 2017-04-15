using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Snapshots;

namespace Eventualize.Console.Domain.TaskList
{
    public class TaskListState : SnapShotBase
    {
        public string Name { get; set; }

        public IEnumerable<Guid> Tasks { get; set; }
    }
}