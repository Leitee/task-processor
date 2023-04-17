namespace TaskProcessor.Core.Shared;

public abstract record TraceableBase
{
    public required Guid TraceId { get; init; }
}

public record HttpRequest : TraceableBase;