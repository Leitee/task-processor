using System;
using System.Text.Json.Serialization;
using TaskProcessor.Common;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Engine
{
	public class TaskMessage : BaseMessage
	{

		[JsonInclude]
		public virtual StepTask CurrentStep { get; private set; }

		[JsonInclude]
		public DateTimeOffset LastUpdate { get; private set; }

		[JsonInclude]
		public MessageStatus Status { get; private set; }

		[JsonInclude]
		public string OperationName { get; private set; }

        [JsonInclude]
		public byte[] Payload { get; private set; }

        public TaskMessage(string operationName) : this(operationName, Guid.NewGuid(), null)
		{
				
		}

        public TaskMessage(string operationName, Guid correlationId) : this(operationName, correlationId, null)
		{
				
		}

        public TaskMessage(string operationName, Guid? correlationId, IDateTimeProvider dateTimeProvider)
			: base(correlationId ?? Guid.NewGuid(), dateTimeProvider)
		{
			if (string.IsNullOrWhiteSpace(operationName))
				throw new ArgumentException(nameof(operationName));

			OperationName = operationName;
			CurrentStep = new StepTask();
			Status = MessageStatus.PROCESSING;
		}

		public void MarkCurrentStepAsCompleted(bool value = true)
		{
			if (value)
			{
				CurrentStep.SetAsCompleted();
				LastUpdate = DateTimeNow;
			}
		}

		public void MarkTaskAsCompleted(bool value = true)
		{
			if (value)
			{
				Status = MessageStatus.COMPLETED;
				LastUpdate = DateTimeNow;
			}
		}

		public void SetErrorAtCurrentStep(string errorMsg)
		{
			CurrentStep.SetFailure(errorMsg);
			LastUpdate = DateTimeNow;
		}

		public void MarkCurrentTaskAsInvalid(bool value = true)
		{
			if (value)
			{
				Status = MessageStatus.DEAD_LETTERED;
				LastUpdate = DateTimeNow;
			}
		}

		public TaskMessage SetPayload<TPayload>(TPayload payload) where TPayload : IPayload
		{
			if (payload is not null)
			{
				Payload = payload.EncodePayload().EncodeToBase64();
				LastUpdate = DateTimeNow;
			}

			return this;
		}

		public TPayload GetPayload<TPayload>() where TPayload : IPayload
			=> Payload is not null
			? Payload.DecodeBase64().DecodeToPayload<TPayload>()
			: default;
	}
}