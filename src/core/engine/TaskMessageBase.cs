using System.Text;
using TaskProcessor.Core.Shared;

namespace TaskProcessor.Core.Engine;

public class TaskMessageBase : MessageBase
{
		private static Encoding DEFAULT_FORMAT = Encoding.UTF8;

        [JsonInclude]
		public StepTask CurrentStep { get; private set; }

		[JsonInclude]
		public DateTime LastUpdate { get; private set; }

		[JsonInclude]
		public string Payload { get; private set; }

		public TaskMessageBase(string payload)
		{
			ArgumentNullException.ThrowIfNullOrEmpty(payload);

			CreationDate = DateTime.Now;
			CurrentStep = new StepTask();
			Payload = payload.EncodeBase64(DEFAULT_FORMAT)!;//TODO: store base64 encoded 
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
			if(isTaskInvalid) 
			{
				CurrentStep.SetAsInvalid();
				LastUpdate = DateTime.Now;
			}
		}

    public TPayload? GetPayload<TPayload>() where TPayload : class 
		=> JsonSerializer.Deserialize<TPayload>(Payload.DecodeBase64(DEFAULT_FORMAT)!);
}