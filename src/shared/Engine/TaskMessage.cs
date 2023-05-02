using System.Text.Json.Serialization;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Engine;

public class TaskMessage : BaseMessage
{
	[JsonInclude]
	public StepTask CurrentStep { get; private set; }

	[JsonInclude]
	public DateTime LastUpdate { get; private set; }

	[JsonInclude]
	public MessageStatus Status { get; private set; }

	[JsonInclude]
	public string OperationName { get; private set; }

	[JsonInclude]
	public byte[]? Payload { get; private set; }

	public TaskMessage(string operationName)
	{
		if (string.IsNullOrWhiteSpace(operationName))
			throw new ArgumentException(nameof(operationName));

		OperationName = operationName;
		CreationDate = DateTime.Now;
		CurrentStep = new StepTask();
		Status = MessageStatus.PROCESSING;
	}

	public void MarkCurrentStepAsCompleted(bool value = true)
	{
		if (value)
		{
			CurrentStep.SetAsCompleted();
			LastUpdate = DateTime.Now;
		}
	}

	public void SetErrorAtCurrentStep(string errorMsg)
	{
		CurrentStep.SetFailure(errorMsg);
		LastUpdate = DateTime.Now;
	}

	public void MarkCurrentTaskAsInvalid(bool value = true)
	{
		if (value)
		{
			CurrentStep.SetAsInvalid();
			Status = MessageStatus.IS_DEADLETTER;
			LastUpdate = DateTime.Now;
		}
	}

	public TaskMessage SetPayload<TPayload>(TPayload payload) where TPayload : IPayload
	{
		if (payload is not null)
		{
			Payload = payload.EncodePayload().EncodeToBase64();
			LastUpdate = DateTime.Now;
		}

		return this;
	}

	public TPayload? GetPayload<TPayload>() where TPayload : IPayload
		=> Payload is not null
		? Payload.DecodeBase64().DecodeToPayload<TPayload>()
		: default;
}

public enum MessageStatus
{
	PROCESSING = 1,
	FINISHED,
	IS_DEADLETTER
}