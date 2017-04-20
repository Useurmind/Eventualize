using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Dapper.Materialization;
using Eventualize.Interfaces.Materialization;
using Eventualize.Materialization;

namespace Eventualize.Console.ReadModel
{
    [Table("dbo", "Tasks")]
    public interface ITaskReadModel : IReadModel
    {
        [Key]
        Guid Id { get; set; }

        string Title { get; set; }

        string Description { get; set; }
    }

    [Table("dbo", "TaskLists")]
    public interface ITaskListReadModel : IReadModel
    {
        [Key]
        Guid Id { get; set; }

        string Name { get; set; }
    }

    [Table("dbo", "TaskListEntries")]
    public interface ITaskListEntryReadModel : IReadModel
    {
        [Key]
        Guid TaskListId { get; set; }

        [Key]
        Guid TaskId { get; set; }

        int Order { get; set; }
    }
}
