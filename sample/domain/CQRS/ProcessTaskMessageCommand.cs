using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.CQRS;

public record ProcessTaskMessageCommand(TaskMessage TaskMessage) : IRequest
{
	public IExecutableStep? ExecutableStep { get; set; }
}

internal class ProcessTaskMessageCommandHandler : IRequestHandler<ProcessTaskMessageCommand>
{
	private readonly ILogger _logger;
	private readonly ITaskExecuter _taskDispatcher;

	public ProcessTaskMessageCommandHandler(ILoggerFactory loggerFactory, ITaskExecuter taskDispatcher)
	{
		_logger = loggerFactory.CreateLogger<ProcessTaskMessageCommandHandler>();
		_taskDispatcher = taskDispatcher;
	}

	public async Task Handle(ProcessTaskMessageCommand request, CancellationToken cancellationToken)
	{
		var taskMessage = request.TaskMessage;
		var executableStep = request.ExecutableStep;
		ArgumentNullException.ThrowIfNull(taskMessage);
		ArgumentNullException.ThrowIfNull(executableStep);

		await _taskDispatcher.ExecuteNextOperation(taskMessage, executableStep, cancellationToken);
	}
}
