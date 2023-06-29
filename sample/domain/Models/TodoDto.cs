using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.Models;
public record TodoDto
{
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public bool Completed { get; set; }
	public DateTimeOffset Duration { get; set; }

	public TodoItem ToEntity() => new(Name, Description, Duration);

}