using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Engine;

#nullable disable
public abstract class TasksEngineDefinitionBase : ITaskEngineDefinition
{
	private readonly LinkedList<IExecutableStep> _taskSteps;

	public IReadOnlyCollection<IExecutableStep> TaskList => _taskSteps.ToList().AsReadOnly();

	public TasksEngineDefinitionBase()
	{
		var tempList = BuildDefinition();
		_taskSteps = tempList?.Any() is true
			? new LinkedList<IExecutableStep>(tempList.OrderBy(ex => ex.ExecutionOrder))
			: throw new ExecutableStepsNotFoundException();
	}

	public abstract IEnumerable<IExecutableStep> BuildDefinition();

	public virtual bool TryGetNextStepTask(StepTask step, out IExecutableStep nextStepTask)
	{
		IExecutableStep currentTask = null;		

		if (step.Name is StepTask.INITIAL_STEP_NAME)
		{
			nextStepTask = _taskSteps.First.Value;
			step.SetNextTask(nextStepTask.Name);
		}
		else if (!string.IsNullOrEmpty(step.Name)
			&& !TryGetExecutableStepByName(step.Name, out currentTask))
		{
			throw new ExecutableStepsNotFoundException();
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
			return false;
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
		return executableStep is not null;
	}
}
