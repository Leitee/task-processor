using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.IO;

public interface IRepositoryIncludable<TEntity> where TEntity : BaseEntity
{
	Task<IQueryable<TEntity>> AllAsync(Expression<Func<TEntity, bool>> predicate,
		Func<IQueryable<TEntity>,
		IOrderedQueryable<TEntity>> orderBy,
		CancellationToken cancellationToken = default,
		params Expression<Func<IIncludable<TEntity>, IIncludable>>[] includes);

	Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate,
		CancellationToken cancellationToken = default,
		params Expression<Func<IIncludable<TEntity>, IIncludable>>[] includes);
}