using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.Models;
public class TodoListDto 
{
	public string Name { get; set; } = string.Empty;
	public IEnumerable<TodoDto> Todos { get; set; } = new List<TodoDto>();

	public TodoListDto FromEntity(TodoList entity)
	{
		Name = entity.Name;
		Todos = entity.Todos.Select(td => new TodoDto
		{
			Name = td.Name,
			Description = td.Description,
			Completed = td.Completed
		});

		return this;
	}
}