using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Infrastructure;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// A factory that will create a meta model for a domain based on attributes on the different aggregate and event classes.
    /// 
    /// It will detect all events marked with the <see cref="IEventData"/> interface and all aggregates marked with the <see cref="IAggregate"/> interface.
    /// Their names and the name of their bounded context will either be resolved using the attributes <see cref="AggregateTypeNameAttribute"/>, <see cref="EventTypeNameAttribute"/>, <see cref="BoundedContextAttribute"/> or the names and namespaces of the types itself.
    /// 
    /// When using the namespaces of the types the following rules apply:
    /// - Aggregate type name is the name of the class of the aggregate
    /// - Event type name is the name of the class of the event
    /// - Bounded context name is the name of the namespace part one level above the direct namespace
    /// 
    /// The last rule is their so all elements that belong to an aggregate can be put into a separate folder (aggregate, state and events).
    /// The parent folder for all aggregate folders is then the name of the bounded context they belong to.
    /// </summary>
    public class ReflectionBasedMetaModelFactory : IDomainMetaModelFactory
    {
        private IEnumerable<Assembly> domainAssemblies;

        /// <summary>
        /// Create an instance of the factory.
        /// </summary>
        /// <param name="domainAssemblies">The assemblies containing the domain model.</param>
        public ReflectionBasedMetaModelFactory(IEnumerable<Assembly> domainAssemblies)
        {
            this.domainAssemblies = domainAssemblies;
        }

        public IDomainMetaModel Build()
        {
            var aggregateTypeRegister = new TypeRegister();
            var eventTypeRegister = new TypeRegister();

            foreach (var assembly in this.domainAssemblies)
            {
                aggregateTypeRegister.ScanTypes(assembly, x => typeof(IAggregate).IsAssignableFrom(x), x => x.FullName);

                eventTypeRegister.ScanTypes(assembly, t => typeof(IEventData).IsAssignableFrom(t), t => t.FullName);
            }

            var aggregateMetaModels = aggregateTypeRegister.GetTypes().Select(
                type =>
                {
                    var aggregateTypeNameAttribute = type.GetCustomAttribute<AggregateTypeNameAttribute>();

                    var boundedContextName = GetBoundedContextName(type);
                    var aggregateTypeName = aggregateTypeNameAttribute == null ? type.Name : aggregateTypeNameAttribute.Name;


                    return new AggregateMetaModel(
                        new BoundedContextName(boundedContextName), 
                        new AggregateTypeName(aggregateTypeName), 
                        type,
                        null
                    );
                });

            var eventMetaModels = eventTypeRegister.GetTypes().Select(
                type =>
                {
                    var eventTypeNameAttribute = type.GetCustomAttribute<EventTypeNameAttribute>();

                    var boundedContextName = GetBoundedContextName(type);
                    var eventTypeName = eventTypeNameAttribute == null ? type.Name : eventTypeNameAttribute.Name;

                    return
                        new EventMetaModel(
                            new BoundedContextName(boundedContextName),
                            new EventTypeName(eventTypeName),
                            type);
                });

            var boundedContextMetaModels =
                aggregateMetaModels.Select(x => x.BoundedContextName).Union(eventMetaModels.Select(x => x.BoundedContextName)).Distinct().Select(
                    boundedContextName =>
                    {
                        return new BoundedContextMetaModel(
                            boundedContextName,
                            aggregateMetaModels.Where(x => x.BoundedContextName == boundedContextName),
                            eventMetaModels.Where(x => x.BoundedContextName == boundedContextName));
                    });

            return new DomainMetaModel(boundedContextMetaModels);
        }

        private static string GetBoundedContextName(Type type)
        {
            var boundedContextNameAttribute = type.GetCustomAttribute<BoundedContextAttribute>();
            return boundedContextNameAttribute == null ? type.FullName.Split('.').Reverse().Skip(2).First() : boundedContextNameAttribute.Name;
        }
    }
}