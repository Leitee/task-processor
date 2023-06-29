using TaskProcessor.Domain.CQRS;
using TaskProcessor.Domain.Operations;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Implementation;

internal class TaskEngineDefinitionInterceptor : IPipelineBehavior<ProcessTaskMessageCommand, Unit>
{
    private readonly TaskEngineDefinitionFactory _engineDefinitionFactory;
    private readonly ILogger _logger;

    public TaskEngineDefinitionInterceptor(TaskEngineDefinitionFactory engineDefinitionFactory, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TaskEngineDefinitionInterceptor>();
        _engineDefinitionFactory = engineDefinitionFactory;
    }

    public async Task<Unit> Handle(ProcessTaskMessageCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
    {
        var taskMessage = request.TaskMessage;
        ArgumentNullException.ThrowIfNull(taskMessage);

        var engineDefinition = _engineDefinitionFactory.GetEngineDefinition<DummyWorkflow>();
        //_logger.LogInformation("Operation '{wf}' for operation '{op}'", nameof(engineDefinition), toProcess.Operation.ToString());

        if (!cancellationToken.IsCancellationRequested
            && engineDefinition.TryGetNextStepTask(taskMessage, out IExecutableStep executableStep))
        {
            _logger.LogInformation("Injecting task: '{taskName}'", executableStep.Name);
            request.ExecutableStep = executableStep;
            await next();
        }

        return Unit.Value;
    }
}
