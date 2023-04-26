using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Engine;

#nullable disable
public abstract class TasksEngineDefinition : ITaskEngineDefinition
{
	protected readonly LinkedList<IExecutableStep> _taskSteps;

	public IReadOnlyCollection<IExecutableStep> TaskList => _taskSteps.ToList().AsReadOnly();

	public TasksEngineDefinition()
	{
		if (_taskSteps?.Any() is true) return;

		var stepList = BuildDefinition();
		_taskSteps = stepList?.Any() is true
			? new LinkedList<IExecutableStep>(stepList.OrderBy(ex => ex.ExecutionOrder))
			: throw new ExecutableStepsNotFoundException();
	}

	protected abstract IEnumerable<IExecutableStep> BuildDefinition();

	public virtual bool TryGetNextStepTask(StepTask step, out IExecutableStep nextStepTask)
	{
		IExecutableStep currentTask = null;

		if (!string.IsNullOrEmpty(step.Name)
			&& !TryGetExecutableStepByName(step.Name, out currentTask))
			throw new ExecutableStepsNotFoundException();

		if (string.IsNullOrEmpty(step.Name) || currentTask is null)
		{
			nextStepTask = _taskSteps.First.Value;
			step.SetNextTask(nextStepTask.Name);
		}
		else if (currentTask?.IsLastStep is true)
		{
			nextStepTask = null;
			return false;
		}
		else if (step.IsCompleted)
		{
			nextStepTask = GetNextTaskExecutableStep(currentTask);
			step.SetNextTask(nextStepTask.Name);
		}
		else if (step.FailedAttempts >= currentTask.MaxRetires)
		{
			nextStepTask = GetFailureHandlerExecutableStep();
		}
		else
		{
			nextStepTask = currentTask;
		}

		return true;
	}

	protected virtual IExecutableStep GetFailureHandlerExecutableStep() 
		=> _taskSteps.Last.Value;

	protected virtual IExecutableStep GetNextTaskExecutableStep(IExecutableStep executableStep) 
		=> _taskSteps.Find(executableStep)?.Next?.Value;

	protected virtual bool IsFinalStep(StepTask step) 
		=> _taskSteps.Last.ValueRef.Equals(step.Name);

	protected virtual bool TryGetExecutableStepByName(string name, out IExecutableStep executableStep)
	{
		executableStep = _taskSteps.FirstOrDefault(t => t.Name == name);
		return executableStep != null;
	}
}
