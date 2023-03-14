using System.Linq.Expressions;

namespace TaskProcessor.Core
{
	public interface IRepository<TEntity, TId> where TEntity : IEntity<TId> where TId : struct
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