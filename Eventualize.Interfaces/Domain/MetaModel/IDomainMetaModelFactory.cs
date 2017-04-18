using System.Linq;

namespace Eventualize.Interfaces.Domain.MetaModel
{
    /// <summary>
    /// Interface for a factory that will create a domain meta model.
    /// </summary>
    public interface IDomainMetaModelFactory
    {
        /// <summary>
        /// Create the meta model for the domain.
        /// </summary>
        /// <returns></returns>
        IDomainMetaModel Build();
    }
}