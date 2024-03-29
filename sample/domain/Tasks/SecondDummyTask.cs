﻿using TaskProcessor.Common;
using TaskProcessor.Domain.Operations;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Tasks;

public class SecondDummyTask : IExecutableStep
{
	public string Name => nameof(SecondDummyTask);

	public string OperationName => nameof(EnrollStudentOperation);

	public byte ExecutionOrder => 2;

	public byte MaxRetires => 2;

	public bool IsLastStep => false;

	public TimeSpan Timeout => TimeSpan.FromSeconds(10);

	public async Task<TaskResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken)
	{
		await Task.Delay(1000, cancellationToken);
		return TaskResult.Success;
	}
}
