namespace TaskProcessor.Core.IO
{
	public interface IRepository<TEntity, TId> where TId : struct
	{
		Task<IQueryable<TEntity>> AllAsync(CancellationToken cancellationToken = default);
		Task<TEntity> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
		Task<TEntity> FindAsync(TId id, CancellationToken cancellationToken = default);
		Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
		Task UpdateAsync(TEntity entityToUpdate, CancellationToken cancellationToken = default);
		Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
		Task DeleteAsync(TEntity entityToDelete, CancellationToken cancellationToken = default);
	}
}