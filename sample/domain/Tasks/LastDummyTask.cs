using TaskProcessor.Domain.Operations;
using TaskProcessor.Shared;
using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Domain.Tasks;

public class LastDummyTask : IExecutableStep
{
	public string Name => nameof(LastDummyTask);

	public string OperationName => nameof(EnrollStudentOperation);

	public byte ExecutionOrder => byte.MaxValue;

	public byte MaxRetires => 1;

	public bool IsLastStep => true;

	public TimeSpan Timeout => TimeSpan.FromSeconds(10);

	public async Task<TaskResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken)
	{
		await Task.Delay(1000);
		return TaskResult.Success;
	}
}
