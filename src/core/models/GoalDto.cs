namespace TaskProcessor.Core.Models;
public record GoalDto
{
	public string Name { get; init; } = string.Empty;
	public string Description { get; init; } = string.Empty;
	public bool Completed { get; init; }
	public DateTimeOffset Duration { get; init; }
}