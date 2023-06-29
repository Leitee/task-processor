using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskProcessor.Domain.Implementation;
using TaskProcessor.Domain.Operations;
using TaskProcessor.Domain.Tasks;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain;
public static class DependencyInjection
{
	public static IServiceCollection AddCoreApplication(this IServiceCollection service, IConfiguration configuration)
	{
		service.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		service.AddSingleton<ITaskDispatcher, TaskDispatcher>();
		service.AddSingleton<ITaskExecuter, TaskExecuter>();

		service.AddSingleton<IExecutableStep, FirstDummyTask>();
		service.AddSingleton<IExecutableStep, SecondDummyTask>();
		service.AddSingleton<IExecutableStep, ThirdDummyTask>();
		service.AddSingleton<IExecutableStep, LastDummyTask>();

		service.AddSingleton<ITaskEngineDefinition, DummyWorkflow>();
		service.AddSingleton<TaskEngineDefinitionFactory>();

		return service;
	}
}