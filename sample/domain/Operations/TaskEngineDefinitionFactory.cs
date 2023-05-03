using System.Collections.Immutable;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Domain.Implementation;

public class TaskEngineDefinitionFactory
{
	private IReadOnlyDictionary<Type, ITaskEngineDefinition> taskByOperationList;

	public TaskEngineDefinitionFactory(IEnumerable<ITaskEngineDefinition> engineDefinitions) 
		=> taskByOperationList = engineDefinitions.ToImmutableDictionary(x => x.GetType());

	public ITaskEngineDefinition GetEngineDefinition<TDefinitionType>() where TDefinitionType : ITaskEngineDefinition
		=> taskByOperationList[typeof(TDefinitionType)];
}
