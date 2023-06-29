using System;

namespace TaskProcessor.Interfaces
{
	public interface IPayload
	{
        public Guid	CorrelationId { get; set; }
    }
}