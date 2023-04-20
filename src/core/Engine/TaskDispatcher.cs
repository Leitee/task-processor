using OneOf;
using OneOf.Types;
using TaskProcessor.Core.IO;
using TaskProcessor.Core.Shared.engine;

namespace TaskProcessor.Core.Engine;

public class TaskDispatcher : ITaskDispatcher
{
	private readonly IPubSubHandler _subHandler;
	private readonly ILogger _logger;

	public TaskDispatcher(IPubSubHandler subHandler, ILoggerFactory loggerFactory)
	{
		_subHandler = subHandler;
		_logger = loggerFactory.CreateLogger<TaskDispatcher>();
	}

	public async Task<OneOf<Success, Error<string>>> DispatchNextOperation(TaskMessage taskMessage, 
		IExecutableStep executableStep, CancellationToken cancellationToken)
	{
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
				executableStep.Name, "Default");// executableStep.WorkFlow.ToString());
			await _subHandler.PublishMessageAsync(taskMessage, cancellationToken);
		}

		return new Error<string>("Consumer task was cancelled");
	}
}
