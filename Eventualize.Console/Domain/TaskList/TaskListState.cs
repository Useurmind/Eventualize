using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Console.Domain.TaskList
{
    public class TaskListState : MementoBase
    {
        public string Name { get; set; }

        public IEnumerable<Guid> Tasks { get; set; }
    }
}