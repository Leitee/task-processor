using OneOf.Types;
using OneOf;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
    public interface ITaskPersistence
	{
		Task<OneOf<TaskResult<TaskMessage>, NotFound>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
		Task<TaskResult<TaskMessage>> SaveMessageAsync(TaskMessage message, CancellationToken cancellationToken);
		Task<TaskResult> RemoveMessageAsync(TaskMessage message, CancellationToken cancellationToken);
	}
}