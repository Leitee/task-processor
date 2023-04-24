using TaskProcessor.Shared;

namespace TaskProcessor.Core.IO;

public interface IEntitiesRepository : IRepository<BaseEntity, Guid>,
	IRepositoryIncludable<BaseEntity>, IRepositoryExtended<BaseEntity>
{

}