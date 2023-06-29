using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using TaskProcessor.Common;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Infrastructure.Persistence
{
	public class EFTaskStore : ITaskPersistence
	{
		private readonly ProcessorDBContexts _processorDBContexts;

		public EFTaskStore(ProcessorDBContexts processorDBContexts)
		{
			_processorDBContexts = processorDBContexts;
		}

		public async Task<OneOf<TaskResult<TaskMessage>, NotFound>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			TaskResult<TaskMessage> taskResult;

			try
			{
				var todos = await _processorDBContexts.Messages
					.AsNoTracking()
					.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
					.ConfigureAwait(false);

				if (todos is null)
					return new NotFound();

				taskResult = todos;
			}
			catch (Exception ex)
			{
				taskResult = TaskResult.ErrorFromException(ex);
			}

			return taskResult;
		}

		public async Task<TaskResult> RemoveMessageAsync(TaskMessage message, CancellationToken cancellationToken)
		{
			try
			{
				_processorDBContexts.Remove<TaskMessage>(message);
				var wasDeleted = await _processorDBContexts.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
				return wasDeleted ? TaskResult.AsSuccess : TaskResult.AsError;
			}
			catch (Exception ex)
			{
				return TaskResult.ErrorFromException(ex);
			}
		}

		public async Task<TaskResult<TaskMessage>> SaveMessageAsync(TaskMessage message, CancellationToken cancellationToken)
		{
			try
			{
				var newPax = await _processorDBContexts.AddAsync(message, cancellationToken);
				var wasSaved = await _processorDBContexts.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
				return wasSaved ? newPax.Entity : TaskResult.AsError;
			}
			catch (Exception ex)
			{
				return TaskResult.ErrorFromException(ex);
			}
		}
	}
}
