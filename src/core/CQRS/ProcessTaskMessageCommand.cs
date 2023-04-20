using TaskProcessor.Core.Engine;
using TaskProcessor.Core.Shared.engine;

namespace TaskProcessor.Core.CQRS;

public record ProcessTaskMessageCommand(TaskMessage TaskMessage) : IRequest
{
	public IExecutableStep? ExecutableStep { get; set; }
}

internal class ProcessTaskMessageCommandHanlder : IRequestHandler<ProcessTaskMessageCommand>
{
	private readonly ILogger _logger;
	private readonly ITaskDispatcher _taskDispatcher;

	public ProcessTaskMessageCommandHanlder(ILoggerFactory loggerFactory, ITaskDispatcher taskDispatcher)
	{
		_logger = loggerFactory.CreateLogger<ProcessTaskMessageCommandHanlder>();
		_taskDispatcher = taskDispatcher;
	}

	public async Task Handle(ProcessTaskMessageCommand request, CancellationToken cancellationToken)
	{
		var taskMessage = request.TaskMessage;
		var executableStep = request.ExecutableStep;
		ArgumentNullException.ThrowIfNull(taskMessage);
		ArgumentNullException.ThrowIfNull(executableStep);

		await _taskDispatcher.DispatchNextOperation(taskMessage, executableStep, cancellationToken);
	}
}
