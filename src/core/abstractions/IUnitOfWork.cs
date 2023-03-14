namespace TaskProcessor.Core;
public interface IUnitOfWork
{
    IEntityFrameworkRepository GetRepository<TEntity>() where TEntity : EntityBase;

    bool Commit();

    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}