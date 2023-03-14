namespace TaskProcessor.Core;
public record GoalDto
{
    public string? Name { get; init; }
    public string? Description { get; init; }
    public bool Completed { get; init; }
    public DateTimeOffset Duration { get; init; }
}