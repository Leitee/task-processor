namespace TaskProcessor.Domain.Models;
public record MilestoneDto
{
	public string Name { get; init; } = string.Empty;
	public List<GoalDto>? Goals { get; init; } = new();
}