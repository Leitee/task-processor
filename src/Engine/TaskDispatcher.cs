using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Engine
{
    public class TaskDispatcher : ITaskDispatcher
	{
		private readonly ITaskPersistence _taskPersistence;
		private readonly ITaskPublisher _messageBroker;

		public TaskDispatcher(ITaskPersistence taskPersistence, ITaskPublisher messageBroker)
		{
			_taskPersistence = taskPersistence;
			_messageBroker = messageBroker;
		}

		public async Task<TaskResult> DispatchNewOperation(TaskMessage taskMessage,
			CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(taskMessage);

			var persistenceResult = await _taskPersistence.SaveMessageAsync(taskMessage, cancellationToken);

			if (persistenceResult.TryPickSuccess(out var storedTaskMessage, out var errorWhenSaving))
				return await _messageBroker.PublishMessageAsync(storedTaskMessage, cancellationToken);

			return errorWhenSaving;
		}
	}
}