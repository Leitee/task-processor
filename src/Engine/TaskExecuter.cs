using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Engine
{
	public class TaskExecuter : ITaskExecuter
	{
		private readonly ILogger _logger;

		public TaskExecuter(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<TaskExecuter>();
		}

		public async Task<TaskResult> ExecuteNextOperation(TaskMessage taskMessage,
			IExecutableStep executableStep, CancellationToken cancellationToken)
		{
			if(executableStep is null || taskMessage is null)
				return TaskResult.ErrorFromMessage($"{nameof(executableStep)} or {nameof(taskMessage)} are null");

			_logger.LogDebug("Executing task [{taskName}] with the following message {@msg}",
				executableStep?.Name, taskMessage);

			var taskCancellationToken = new CancellationTokenSource(executableStep.Timeout).Token;
			var execResult = await executableStep.ExecuteAsync(taskMessage, taskCancellationToken);

			if(execResult.TryPickInvalid(out var operationException, out TaskResult taskResult))
			{
				taskMessage.MarkCurrentTaskAsInvalid();
				return TaskResult.ErrorFromMessage(operationException.Message);
			}

			if(taskResult.TryPickError(out var error, out _))
			{
				_logger.LogError("Task [{taskName}] failed with error [{error}]", executableStep.Name, error.Value);
				taskMessage.SetErrorAtCurrentStep(error.Value);
				return taskResult;
			}

			_logger.LogInformation("Task [{taskName}] completed successfully", executableStep.Name);
			taskMessage.MarkCurrentStepAsCompleted();
			return taskResult;
		}
	}
}