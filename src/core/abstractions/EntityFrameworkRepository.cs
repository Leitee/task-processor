using TaskProcessor.Core.Shared;
using TaskProcessor.Core.Shared.Interfaces;

namespace TaskProcessor.Core.Abstractions;

public interface IEntityFrameworkRepository : IRepository<BaseEntity,
	Guid>, IRepositoryIncludable<BaseEntity>, IRepositoryExtended<BaseEntity>
{

}