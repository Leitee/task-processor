namespace TaskProcessor.Core.Shared.engine;

public abstract class BaseMessage
{
	protected BaseMessage()
		: this(Guid.NewGuid(), DateTime.UtcNow) { }

	[JsonConstructor]
	protected BaseMessage(Guid id, DateTime createDate)
	{
		Id = id;
		CreationDate = createDate;
	}

	[JsonInclude]
	public Guid Id { get; init; }

	[JsonInclude]
	public DateTime CreationDate { get; init; }
}