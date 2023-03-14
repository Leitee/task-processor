using MediatR;

namespace TaskProcessor.Core;
public abstract class EventBase : INotification
{
      public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}