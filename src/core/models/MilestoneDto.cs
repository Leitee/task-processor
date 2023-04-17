namespace TaskProcessor.Core.Models;
public record MilestoneDto
{
	public string Name { get; init; } = string.Empty;
	public List<GoalDto>? Goals { get; init; } = new();
}