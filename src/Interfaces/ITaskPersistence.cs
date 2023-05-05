using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
    public interface ITaskPersistence
	{
		Task<TaskResult<TaskMessage>> GetByKeyAsync(Guid key, CancellationToken cancellationToken);
		Task<TaskResult<TaskMessage>> SaveMessageAsync(TaskMessage message, CancellationToken cancellationToken);
		Task<TaskResult> RemoveMessageAsync(TaskMessage message, CancellationToken cancellationToken);
	}
}