using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.IO;

namespace TaskProcessor.Domain.Abstractions;
public interface IUnitOfWork
{
	IEntitiesRepository GetRepository<TEntity>() where TEntity : BaseEntity;

	ITasksStore GetTasksStore();

	bool Commit();

	Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}