using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskProcessor.Domain.IO;
using TaskProcessor.Infrastructure.Message;
using TaskProcessor.Infrastructure.Persistence;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddMassTransit(opt => {
            opt.SetKebabCaseEndpointNameFormatter();
            opt.SetInMemorySagaRepositoryProvider();
            var assembly = Assembly.GetEntryAssembly();
            opt.AddConsumers(assembly);
            opt.AddSagaStateMachines(assembly);
            opt.AddSagas(assembly);
            opt.AddActivities(assembly);
            opt.UsingInMemory((ctx, cfg) => cfg.ConfigureEndpoints(ctx));
        });

		service.AddSingleton<ITaskPublisher, InMemoryPubSubHandler>();
		service.AddSingleton<IPubSubHandler, InMemoryPubSubHandler>();
        service.AddSingleton<ITaskPersistence, InMemoryTaskPersistence>();

        return service;
    }
}