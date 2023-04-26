namespace TaskProcessor.Shared;

internal class ExecutableStepsNotFoundException : Exception
{
	public ExecutableStepsNotFoundException()
		: base("Task are not present to start up engine.")
	{
	}
}
