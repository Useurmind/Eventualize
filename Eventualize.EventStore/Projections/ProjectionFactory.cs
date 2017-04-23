using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;

using Eventualize.EventStore.Persistence;
using Eventualize.EventStore.Projections;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.EventStore.Test.Projections
{
    public class ProjectionFactory : IProjectionFactory
    {
        private ProjectionsManager projectionsManager;

        private UserCredentials userCredentials;

        public ProjectionFactory(ProjectionsManager projectionsManager, UserCredentials userCredentials )
        {
            this.projectionsManager = projectionsManager;
            this.userCredentials = userCredentials;
        }

        public void EnsureProjectionFor(IDomainMetaModel domainMetaModel)
        {
            this.EnsureProjectionForAllBoundedContexts();
            foreach (var boundedContext in domainMetaModel.BoundedContexts)
            {
                this.EnsureProjectionFor(boundedContext.BoundedContextName);
            }
        }

        /// <summary>
        /// Ensures that there is a projection that merges events from all bounded contexts.
        /// </summary>
        public void EnsureProjectionForAllBoundedContexts()
        {
            var projectionName = new ProjectionStreamName();
            this.ExecuteOnlyIfProjectionDoesNotExist(
                projectionName,
                p =>
                {
                    var streamPrefix = AggregateStreamName.AggregatePrefix;
                    this.CreateProjectionForStreamsStartingWith(p, streamPrefix);
                });
        }

        /// <summary>
        /// Ensures that there is a projection that merges all events from the given bounded context.
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context.</param>
        public void EnsureProjectionFor(BoundedContextName boundedContextName)
        {
            var projectionName = new ProjectionStreamName(boundedContextName);
            this.ExecuteOnlyIfProjectionDoesNotExist(
                projectionName,
                p =>
                {
                    var streamPrefix = AggregateStreamName.GetBoundedContextPrefix(boundedContextName);
                    this.CreateProjectionForStreamsStartingWith(p, streamPrefix);
                });
        }

        /// <inheritdoc />
        public void EnsureProjectionFor(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName)
        {
            var projectionName = new ProjectionStreamName(boundedContextName, aggregateTypeName);
            this.ExecuteOnlyIfProjectionDoesNotExist(
                projectionName,
                p =>
                {
                    var streamPrefix = AggregateStreamName.GetAggregateTypePrefix(boundedContextName, aggregateTypeName);
                    this.CreateProjectionForStreamsStartingWith(p, streamPrefix);
                });
        }

        private void ExecuteOnlyIfProjectionDoesNotExist(ProjectionStreamName projectionName, Action<string> action)
        {
            var existingProjections = this.projectionsManager.ListContinuousAsync(this.userCredentials).Result;
            if (!existingProjections.Any(x => x.Name == projectionName.ToString()))
            {
                action(projectionName.ToString());
            }
        }

        private void CreateProjectionForStreamsStartingWith(string projectionName, string streamPrefix)
        {
            var query = string.Format(@"fromAll().when({{$any : function(s,e) {{
if (e.streamId.indexOf('{0}') === 0) {{
    linkTo('{1}', e);
        }}
    }}
}});", streamPrefix, projectionName);

            this.projectionsManager.CreateContinuousAsync(projectionName, query, true, this.userCredentials).Wait();
        }
    }
}
