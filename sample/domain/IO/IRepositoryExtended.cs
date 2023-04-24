using TaskProcessor.Shared;

namespace TaskProcessor.Domain.IO;

public interface IRepositoryExtended<TEntity> where TEntity : BaseEntity
{
	Task<int> GetCountAsync(CancellationToken cancellationToken = default);
	Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
	Task<int> GetCountByExpressionAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
	Task<IQueryable<TEntity>> ExecuteQueryAsync(string query, CancellationToken cancellationToken = default, params object[] paramaters);
	Task<List<TEntity>> ExecuteStoredProcedure(string spName, CancellationToken cancellationToken = default, params object[] parameters);
	Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}