using TaskProcessor.Domain.Operations;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;
using TaskProcessor.Common;

namespace TaskProcessor.Domain.Tasks;

public class ThirdDummyTask : IExecutableStep
{
	public string Name => nameof(ThirdDummyTask);

	public string OperationName => nameof(EnrollStudentOperation);

	public byte ExecutionOrder => 3;

	public byte MaxRetires => 2;

	public bool IsLastStep => true;

	public TimeSpan Timeout => TimeSpan.FromSeconds(10);

	public async Task<TaskResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken)
	{
		await Task.Delay(1000, cancellationToken);
		return TaskResult.Success;
	}

	Task<TaskResult> IExecutableStep.ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
