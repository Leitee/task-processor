
namespace TaskProcessor.Core.Engine;

public abstract class MessageBase
{
    protected MessageBase()
        : this(Guid.NewGuid(), DateTime.UtcNow) { }

    [JsonConstructor]
    protected MessageBase(Guid id, DateTime createDate)
    {
        Id = id;
        CreationDate = createDate;
    }

    [JsonInclude]
    public Guid Id { get; init; }

    [JsonInclude]
    public DateTime CreationDate { get; init; }
}