using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.Models;
using TaskProcessor.Domain.Operations;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.CQRS;

public record UpdateTodoListCommand(TodoListDto TodoList) : IRequest;

internal class EnrollTaskMessageCommandHandler : IRequestHandler<UpdateTodoListCommand>
{
	private readonly ILogger _logger;
	private readonly ITaskDispatcher _taskDispatcher;

	public EnrollTaskMessageCommandHandler(ILoggerFactory loggerFactory, ITaskDispatcher taskDispatcher)
	{
		_logger = loggerFactory.CreateLogger<EnrollTaskMessageCommandHandler>();
		_taskDispatcher = taskDispatcher;
	}

	public async Task Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
	{
		var payload = new TodoList(request.TodoList.Name).SetTodos(request.TodoList.Todos.Select(td => td.ToEntity()));
		var taskMessage = new TaskMessage(DummyWorkflow.OPERATION_NAME).SetPayload(payload);
		await _taskDispatcher.DispatchNewOperation(taskMessage, cancellationToken);
	}
}
