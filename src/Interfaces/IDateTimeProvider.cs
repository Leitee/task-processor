using System;

namespace TaskProcessor.Interfaces
{
	public interface IDateTimeProvider
	{
		DateTimeOffset Now();
		DateTimeOffset UtcNow();
	}
}
