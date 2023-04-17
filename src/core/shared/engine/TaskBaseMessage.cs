using System.Text;
using TaskProcessor.Core.Shared.Interfaces;

namespace TaskProcessor.Core.Shared.engine;

public class TaskBaseMessage<TPayload> : BaseMessage where TPayload : IPayload
{
	private static Encoding DEFAULT_FORMAT = Encoding.UTF8;

	[JsonInclude]
	public StepTask CurrentStep { get; private set; }

	[JsonInclude]
	public DateTime LastUpdate { get; private set; }

	[JsonInclude]
	public string Payload { get; private set; }

	public TaskBaseMessage(TPayload payload)
	{
		ArgumentNullException.ThrowIfNull(payload);

		CreationDate = DateTime.Now;
		CurrentStep = new StepTask();
		Payload = JsonSerializer.Serialize(payload).EncodeBase64(DEFAULT_FORMAT)!;//TODO: store base64 encoded 
	}

	public void MarkCurrentStepAsCompleted(bool result = true)
	{
		if (result)
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

	public void MarkCurrentTaskAsInvalid(bool isTaskInvalid = true)
	{
		if (isTaskInvalid)
		{
			CurrentStep.SetAsInvalid();
			LastUpdate = DateTime.Now;
		}
	}

	public TPayload? GetPayload()
		=> JsonSerializer.Deserialize<TPayload>(Payload.DecodeBase64(DEFAULT_FORMAT));
}