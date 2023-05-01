using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces;

public interface ITaskEngineDefinition
{
    IReadOnlyCollection<IExecutableStep> TaskList { get; }
	IEnumerable<IExecutableStep> BuildDefinition();
	bool TryGetNextStepTask(StepTask step, out IExecutableStep nextStepTask);
}
