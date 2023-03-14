using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProcessor.Core;

public interface IEntity<TId> where TId : struct
{
    public TId Id { get; set; }
}

public abstract class EntityBase : IEntity<Guid>
{
    public Guid Id { get; set; }

    private readonly List<EventBase> _domainEvents = new();

    protected EntityBase(Guid id) => Id = id;

    public EntityBase() { }


    [NotMapped]
    public IReadOnlyCollection<EventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(EventBase domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(EventBase domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}