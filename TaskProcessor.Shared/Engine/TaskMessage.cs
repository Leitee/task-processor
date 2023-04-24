using System.Text.Json.Serialization;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Engine;

public sealed class TaskMessage : BaseMessage
{
	[JsonInclude]
	public StepTask CurrentStep { get; private set; }

	[JsonInclude]
	public DateTime LastUpdate { get; private set; }

	[JsonInclude]
	public bool IsDeadLetter { get; private set; }

	[JsonInclude]
	public byte[] Payload { get; private set; }

	public TaskMessage(IPayload payload)
	{
		ArgumentNullException.ThrowIfNull(payload);

		CreationDate = DateTime.Now;
		CurrentStep = new StepTask();
		Payload = payload.EncodePayload().EncodeToBase64();
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
		CurrentStep.SetAsFailure(errorMsg);
		LastUpdate = DateTime.Now;
	}

	public void MarkCurrentTaskAsInvalid(bool value = true)
	{
		if (value)
		{
			CurrentStep.SetAsInvalid();
			LastUpdate = DateTime.Now;
		}
	}

	public void MarkAsDeadLetter(bool value = true)
	{
		IsDeadLetter = value;
		LastUpdate = DateTime.Now;
	}

	public TPayload? GetPayload<TPayload>() where TPayload : IPayload
		=> Payload.DecodeBase64().DecodeToPayload<TPayload>();
}