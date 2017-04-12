using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Console.Domain
{
    public class TaskState : MementoBase
    {
        public string Title { get; set; }

        public string Description { get; set; }
    }
}