using System;
using System.Text.Json.Serialization;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Common
{
	public abstract class BaseMessage
    {
		private readonly IDateTimeProvider _dateTimeProvider;

        protected DateTimeOffset DateTimeNow => _dateTimeProvider?.UtcNow() ?? DateTimeOffset.UtcNow;

		[JsonConstructor]
        protected BaseMessage(Guid id, IDateTimeProvider dateTimeProvider)
        {
            Id = id;
            _dateTimeProvider = dateTimeProvider;
            CreationDate = DateTimeNow;
        }

        [JsonInclude]
        public Guid Id { get; init; }

        [JsonInclude]
        public DateTimeOffset CreationDate { get; init; }
    }

	public enum MessageStatus
	{
		PROCESSING = 1,
		COMPLETED,
		FAILED,
		DEAD_LETTERED
	}
}