namespace TaskProcessor.Shared.Engine;

public sealed class StepTask : ValueObject
{
	public const string INITIAL_STEP_NAME = "STEP_0";

	public string Name { get; private set; } = INITIAL_STEP_NAME;
	public byte Order { get; private set; } = 0;
	public bool IsCompleted { get; private set; } = false;
	public byte FailedAttempts { get; private set; } = 0;
	public string ErrorMessage { get; private set; } = string.Empty;

	public void SetNextTask(string stepTask)
	{
		Order++;
		Name = stepTask;
		IsCompleted = false;
		FailedAttempts = 0;
		ErrorMessage = string.Empty;
	}

	public void SetAsCompleted() => IsCompleted = true;

	public void SetAsFailure(string errorMsg)
	{
		FailedAttempts++;
		ErrorMessage = errorMsg;
	}

	public void SetAsInvalid()
	{
		FailedAttempts = byte.MaxValue;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Name;
		yield return Order;
		yield return IsCompleted;
		yield return FailedAttempts;
	}

	public StepTask Clone()
	{
		return new StepTask
		{
			Name = Name,
			Order = Order,
			IsCompleted = IsCompleted,
			FailedAttempts = FailedAttempts,
			ErrorMessage = ErrorMessage
		};
	}
}
