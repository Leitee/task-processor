using System.Collections.Generic;
using System.Linq;
using TaskProcessor.Common;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Engine
{
	public abstract class TaskEngineDefinitionBase : ITaskEngineDefinition
	{
		private readonly LinkedList<IExecutableStep> _stepTasks;

		public TaskEngineDefinitionBase(IEnumerable<IExecutableStep> executableSteps)
		{
			var tempList = BuildDefinition(executableSteps.Where(x => x is not IFailureStep));
			_stepTasks = tempList?.Any() is true
				? new LinkedList<IExecutableStep>(tempList.OrderBy(ex => ex.ExecutionOrder))
				: throw new ExecutableStepsNotFoundException();
		}

		public abstract IEnumerable<IExecutableStep> BuildDefinition(IEnumerable<IExecutableStep> executableSteps);
		public abstract TaskMessage BuildMessageWithPayload(IPayload payload);

		public virtual bool TryGetNextStepTask(TaskMessage taskMessage, out IExecutableStep nextStepTask)
		{
			IExecutableStep currentTask = null;
			var step = taskMessage.CurrentStep;

			if (step.Name is StepTask.INITIAL_STEP_NAME)
			{
				nextStepTask = _stepTasks.First.Value;
				step.SetNextTask(nextStepTask.Name);
			}
			else if (!string.IsNullOrEmpty(step.Name)
				&& !TryGetExecutableStepByName(step.Name, out currentTask))
			{
				throw new ExecutableStepsNotFoundException();
			}
			else if (step.IsCompleted)
			{
				nextStepTask = GetNextTaskExecutableStep(currentTask);
				step.SetNextTask(nextStepTask.Name);
			}
			else
			{
				nextStepTask = currentTask;
			}

			return true;
		}		

		protected virtual IExecutableStep GetNextTaskExecutableStep(IExecutableStep executableStep)
			=> _stepTasks.Find(executableStep)?.Next?.Value;

		protected virtual bool TryGetExecutableStepByName(string name, out IExecutableStep executableStep)
		{
			executableStep = _stepTasks.FirstOrDefault(t => t.Name == name);
			return executableStep is not null;
		}
	}
}