using TaskProcessor.Core.Shared;
using TaskProcessor.Core.Shared.Interfaces;

namespace TaskProcessor.Core.Abstractions;

public interface IEntitiesRepository : IRepository<BaseEntity,	Guid>, 
	IRepositoryIncludable<BaseEntity>, IRepositoryExtended<BaseEntity>
{

}