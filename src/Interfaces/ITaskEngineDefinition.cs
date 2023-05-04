using System.Collections.Generic;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
	public interface ITaskEngineDefinition
	{
		IReadOnlyCollection<IExecutableStep> TaskList { get; }
		IEnumerable<IExecutableStep> BuildDefinition();
		bool TryGetNextStepTask(StepTask step, out IExecutableStep nextStepTask);
	}
}