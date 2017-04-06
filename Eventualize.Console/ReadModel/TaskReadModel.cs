using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Dapper.Materialization;
using Eventualize.Materialization;

namespace Eventualize.Console.ReadModel
{
    [Table("dbo", "Tasks")]
    public class TaskReadModel : IReadModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public long Version { get; set; }

        public DateTime LastEventDate { get; set; }

        public long LastEventStoreIndex { get; set; }

        public string LastModifierId { get; set; }
    }
}
