namespace TaskProcessor.Core;
public record MilestoneDto
{
    public string? Name { get; init; }
    public List<GoalDto>? Goals { get; init; }
}