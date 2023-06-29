using OneOf;
using OneOf.Types;
using TaskProcessor.Common;
using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.IO
{
	public interface ITodoRepository
	{
		Task<TaskResult<TodoList>> CreateAsync(TodoList todo, CancellationToken cancellationToken);
		Task<TaskResult> DeleteAsync(TodoList todo, CancellationToken cancellationToken);
		Task<TaskResult<IEnumerable<TodoList>>> GetAllTodoListAsync(CancellationToken cancellationToken);
		Task<OneOf<TaskResult<TodoList>, NotFound>> GetTodoListByIdAsync(Guid id, CancellationToken cancellationToken);
	}
}
