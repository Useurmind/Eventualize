using System.Linq;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IMaterializationFactory
    {
        IFluentProjectionMaterialization<TProjectionModel> Model<TProjectionModel>() 
            where TProjectionModel : IProjectionModel;
    }
}