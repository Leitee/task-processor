using TaskProcessor.Core.Shared;

namespace TaskProcessor.Core.Abstractions;
public interface IUnitOfWork
{
	IEntityFrameworkRepository GetRepository<TEntity>() where TEntity : BaseEntity;

	bool Commit();

	Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}