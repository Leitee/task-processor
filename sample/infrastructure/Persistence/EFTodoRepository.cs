using Microsoft.EntityFrameworkCore;
using OneOf;
using OneOf.Types;
using TaskProcessor.Common;
using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.IO;

namespace TaskProcessor.Infrastructure.Persistence
{
	public class EFTodoRepository : ITodoRepository
	{
		private readonly LocalDBContexts _localDBContexts;

		public EFTodoRepository(LocalDBContexts localDBContexts)
		{
			_localDBContexts = localDBContexts;
		}

		public async Task<TaskResult> DeleteAsync(TodoList passenger, CancellationToken cancellationToken)
		{
			try
			{
				_localDBContexts.Remove<TodoList>(passenger);
				var wasDeleted = await _localDBContexts.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
				return wasDeleted ? TaskResult.AsSuccess : TaskResult.AsError;
			}
			catch (Exception ex)
			{
				return TaskResult.ErrorFromException(ex);
			}
		}

		public async Task<TaskResult<TodoList>> CreateAsync(TodoList passenger, CancellationToken cancellationToken)
		{
			try
			{
				var newPax = await _localDBContexts.AddAsync(passenger, cancellationToken);
				var wasSaved = await _localDBContexts.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
				return wasSaved ? newPax.Entity : TaskResult.AsError;
			}
			catch (Exception ex)
			{
				return TaskResult.ErrorFromException(ex);
			}
		}

		public async Task<OneOf<TaskResult<TodoList>, NotFound>> GetTodoListByIdAsync(Guid id, CancellationToken cancellationToken)
		{
			TaskResult<TodoList> taskResult;

			try
			{
				var todos = await _localDBContexts.TodoLists
					.AsNoTracking()
					.FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
					.ConfigureAwait(false);

				if(todos is null)
					return new NotFound();

				taskResult = todos;
			}
			catch (Exception ex)
			{
				taskResult = TaskResult.ErrorFromException(ex);
			}

			return taskResult;
		}

		public async Task<TaskResult<IEnumerable<TodoList>>> GetAllTodoListAsync(CancellationToken cancellationToken)
		{
			TaskResult<IEnumerable<TodoList>> taskResult;

			try
			{
				var todos = await _localDBContexts.TodoLists
					.AsNoTracking()
					.ToListAsync()
					.ConfigureAwait(false);

				taskResult = todos;
			}
			catch (Exception ex)
			{
				taskResult = TaskResult.ErrorFromException(ex);
			}

			return taskResult;
		}
	}
}
