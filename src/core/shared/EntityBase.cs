using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskProcessor.Core;
public abstract class EntityBase
{
    public Guid Id { get; set; }

    private readonly List<BaseEvent> _domainEvents = new();

		protected EntityBase(Guid id) => Id = id;

		public EntityBase() { }


		[NotMapped]
		public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

		public void AddDomainEvent(BaseEvent domainEvent)
		{
			_domainEvents.Add(domainEvent);
		}

		public void RemoveDomainEvent(BaseEvent domainEvent)
		{
			_domainEvents.Remove(domainEvent);
		}

		public void ClearDomainEvents()
		{
			_domainEvents.Clear();
		}
}