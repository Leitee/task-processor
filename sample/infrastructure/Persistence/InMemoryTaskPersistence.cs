using TaskProcessor.Common;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Infrastructure.Persistence
{
	public class InMemoryTaskPersistence : ITaskPersistence
	{
		private IDictionary<Guid, TaskMessage> dataBase;

		public InMemoryTaskPersistence()
		{
			dataBase = new Dictionary<Guid, TaskMessage>();
		}

		public async Task<TaskResult> RemoveMessageAsync(TaskMessage message, CancellationToken cancellationToken)
		{
			try
			{
				await Task.Run(() =>
				{
					dataBase.Remove(message.Id);
				},
				cancellationToken)
					.ConfigureAwait(false);

				return TaskResult.Success;
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
				await Task.Run(() =>
				{
					dataBase.Add(message.Id, message);
				},
				cancellationToken)
					.ConfigureAwait(false);

				return message;
			}
			catch (Exception ex)
			{
				return TaskResult.ErrorFromException(ex);
			}
		}

		public async Task<TaskResult<TaskMessage?>> GetByKeyAsync(Guid key, CancellationToken cancellationToken)
		{
			TaskMessage? dataRestult = null;

			await Task.Run(() =>
			{
				dataRestult = dataBase[key];
			},
			cancellationToken)
				.ConfigureAwait(false);

			return await Task.FromResult(dataRestult);
		}
	}
}
