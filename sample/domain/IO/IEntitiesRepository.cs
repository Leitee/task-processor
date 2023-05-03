using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.IO;

public interface IEntitiesRepository : IRepository<BaseEntity, Guid>,
	IRepositoryIncludable<BaseEntity>, IRepositoryExtended<BaseEntity>
{

}