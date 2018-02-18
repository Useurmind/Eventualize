using System;
using System.Linq;

using Eventualize.Projection.ProjectionMetaModel;

namespace Eventualize.Projection.FluentProjection
{
    public interface IFluentTopicProjection
    {
        /// <summary>
        /// Start to define a projection of the topic into the given model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model in which you want to project.</typeparam>
        /// <param name="defineProjection">Define the projection for the given model.</param>
        IFluentTopicProjection ProjectTo<TModel>(Action<IFluentProjection<TModel>> defineProjection);

        /// <summary>
        /// Finish the topic projection and build a model from it.
        /// </summary>
        /// <returns></returns>
        TopicModel Build();
    }
}