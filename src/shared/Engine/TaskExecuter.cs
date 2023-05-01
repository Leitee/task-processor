using Microsoft.Extensions.Logging;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Engine;

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
			return TaskResult.Error;
		}

		_logger.LogDebug("Executing task [{taskNamee}] with the following message {@msg}",
			executableStep.Name, taskMessage);

		var taskCancellatiomToken = new CancellationTokenSource(executableStep.Timeout).Token;
		var result = await executableStep.ExecuteAsync(taskMessage, taskCancellatiomToken);

		_logger.LogInformation(result.ToString());

		result.Switch(
			ok => taskMessage.MarkCurrentStepAsCompleted(),
			error => taskMessage.SetErrorAtCurrentStep(error.Value)
			);

		if (!executableStep.IsLastStep)
		{
			_logger.LogInformation("Publishing - [{taskName}] as is not the last one for flow [{flow}]",
				executableStep.Name, "Default"); // executableStep.WorkFlow.ToString());
			return await _publisher.PublishMessageAsync(taskMessage, cancellationToken);
		}

		return TaskResult.Success;
	}
}
