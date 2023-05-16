using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Engine
{
    public class TaskExecuter : ITaskExecuter
	{
		private readonly ITaskPublisher _publisher;
		private readonly ILogger _logger;

		public TaskExecuter(ITaskPublisher publisher, ILoggerFactory loggerFactory)
		{
			_publisher = publisher;
			_logger = loggerFactory.CreateLogger<TaskExecuter>();
		}

		public async Task<TaskResult> ExecuteNextOperation(TaskMessage taskMessage,
			IExecutableStep executableStep, CancellationToken cancellationToken)
		{
			if (taskMessage is null || executableStep is null)
			{
				_logger.LogError("Some parameter are null [{@msg}] and [{@exe}]", taskMessage, executableStep);
				return TaskResult.AsError;
			}

			_logger.LogDebug("Executing task [{taskName}] with the following message {@msg}",
				executableStep.Name, taskMessage);

			var taskCancellationToken = new CancellationTokenSource(executableStep.Timeout).Token;
			var result = await executableStep.ExecuteAsync(taskMessage, taskCancellationToken);

			if(result.TryPickInvalid(out var operationException, out TaskResult taskResult))
			{
				taskMessage.MarkCurrentTaskAsInvalid();
				return TaskResult.AsError;
			}

			taskResult.Switch(
				ok => taskMessage.MarkCurrentStepAsCompleted(),
				error => taskMessage.SetErrorAtCurrentStep(error.Value)
			);

			if (!executableStep.IsLastStep)
			{
				_logger.LogInformation("Publishing - [{taskName}] as is not the last one for flow [{flow}]",
					executableStep.Name, "Default"); // executableStep.WorkFlow.ToString());
				return await _publisher.PublishMessageAsync(taskMessage, cancellationToken);
			}

			return taskResult;
		}
	}
}