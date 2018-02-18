using System;
using System.Linq;

namespace Eventualize.Projection.FluentProjection
{
    /// <summary>
    /// Extensions for the <see cref="ITopic"/> interface.
    /// </summary>
    public static class TopicExtensions
    {
        /// <summary>
        /// Start to define a projection of the topic into the given model type.
        /// </summary>
        /// <typeparam name="TModel">The type of the model in which you want to project.</typeparam>
        /// <param name="topic">The topic that should be projected.</param>
        /// <param name="defineProjection">Define the projection for the given model.</param>
        public static IFluentTopicProjection ProjectTo<TModel>(this ITopic topic, Action<IFluentProjection<TModel>> defineProjection)
        {
            return new TopicContext(topic).ProjectTo(defineProjection);
        }
    }
}