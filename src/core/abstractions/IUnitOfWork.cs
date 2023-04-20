using TaskProcessor.Core.IO;
using TaskProcessor.Core.Shared;

namespace TaskProcessor.Core.Abstractions;
public interface IUnitOfWork
{
	IEntitiesRepository GetRepository<TEntity>() where TEntity : BaseEntity;

	ITasksStore GetTasksStore();

	bool Commit();

	Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}