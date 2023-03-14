namespace TaskProcessor.Core;

public interface IEntityFrameworkRepository : IRepository<EntityBase, 
    Guid>, IRepositoryIncludable<EntityBase>, IRepositoryExtended<EntityBase>
{
    
}