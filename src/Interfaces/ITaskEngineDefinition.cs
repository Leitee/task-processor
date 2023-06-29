using System.Collections.Generic;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
	public interface ITaskEngineDefinition
	{
		IEnumerable<IExecutableStep> BuildDefinition(IEnumerable<IExecutableStep> executableSteps);
		bool TryGetNextStepTask(TaskMessage task, out IExecutableStep nextStepTask);
		TaskMessage BuildMessageWithPayload(IPayload payload);
	}
}