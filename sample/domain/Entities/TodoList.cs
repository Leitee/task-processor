using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Entities;
public class TodoList : BaseEntity, IPayload
{
	public string Name { get; private set; }
	public IEnumerable<TodoItem> Todos { get; private set; }
	public Guid CorrelationId { get; set; }

	public TodoList(string name)
	{
		Name = name;
		Todos = Enumerable.Empty<TodoItem>();
	}

	public TodoList SetTodos(IEnumerable<TodoItem> todos)
	{
		if (todos?.Any() is true)
		{
			Todos = todos;
			return this;
		}

		throw new ArgumentNullException(nameof(todos));
	}
}