using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

using Eventualize.Projection.ProjectionMetaModel;

namespace Eventualize.Projection.FluentProjection
{
    public class TopicContext : IFluentTopicProjection
    {
        private TopicModel topicModel;

        public TopicContext(ITopic topic)
        {
            this.topicModel = new TopicModel()
                                  {
                                      Topic = topic
                                  };
        }

        public IFluentTopicProjection ProjectTo<TProjectionModel>(Action<IFluentProjection<TProjectionModel>> defineProjection)
        {
            if (this.topicModel.Projections.Any(x => x.ProjectionModelType == typeof(TProjectionModel)))
            {
                throw new Exception($"The model type {typeof(TProjectionModel).FullName} was already projected for topic {this.topicModel.Topic.Name}");
            }

            var projectionContext = new ProjectionContext<TProjectionModel>(this.topicModel.Topic);

            defineProjection(projectionContext);

            this.topicModel.Projections = this.topicModel.Projections.Union(new[] { projectionContext.Build() });

            return this;
        }

        public TopicModel Build()
        {
            return this.topicModel;
        }
    }
}
