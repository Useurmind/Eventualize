using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class MaterializationFactory : IMaterializationFactory
    {
        private FluentMaterializationContext context;

        public MaterializationFactory(IEventualizeContainerBuilder containerBuilder)
        {
            this.context= new FluentMaterializationContext(this, containerBuilder);
        }

        public IFluentProjectionMaterialization<TProjectionModel> Model<TProjectionModel>()
            where TProjectionModel : IProjectionModel
        {
            return new FluentProjectionMaterialization<TProjectionModel>(this.context);
        }
    }
}
