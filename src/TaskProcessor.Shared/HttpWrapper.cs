namespace TaskProcessor.Shared;

public abstract record TraceableBase
{
	public Guid TraceId { get; init; }
}

public record HttpWrapper : TraceableBase;