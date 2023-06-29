using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.IO;
using TaskProcessor.Domain.Models;

namespace TaskProcessor.Domain.CQRS;

public record CreateTodoListCommand(string Name, IEnumerable<TodoDto> Todos) : IRequest<Guid>;

internal class CreateMilestoneHandler : IRequestHandler<CreateTodoListCommand, Guid>
{
	private readonly ITodoRepository _todoRepository;

	public CreateMilestoneHandler(ITodoRepository todoRepository)
	{
		_todoRepository = todoRepository;
	}

	public async Task<Guid> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
	{
		var newMilestone = new TodoList(request.Name)
			.SetTodos(request.Todos.Select(td => td.ToEntity()));

		var entity = await _todoRepository.CreateAsync(newMilestone, cancellationToken);

		return entity.IsSuccess ? entity.AsT0.Id : Guid.Empty;
	}
}
