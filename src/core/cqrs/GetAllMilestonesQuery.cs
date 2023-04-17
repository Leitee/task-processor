
using AutoMapper;
using AutoMapper.QueryableExtensions;
using TaskProcessor.Core.Abstractions;
using TaskProcessor.Core.Entities;
using TaskProcessor.Core.Models;

namespace TaskProcessor.Core.cqrs;

public record GetAllMilestonesResponse(IEnumerable<MilestoneDto> Milestones);

public record GetAllMilestonesQuery : IRequest<GetAllMilestonesResponse>;

internal class GetAllMilestonesHandler : IRequestHandler<GetAllMilestonesQuery, GetAllMilestonesResponse>
{
	private readonly IUnitOfWork _uow;
	private readonly IConfigurationProvider _autoMapperConfig;

	public GetAllMilestonesHandler(IUnitOfWork uow, IConfigurationProvider autoMapperConfig)
	{
		_uow = uow;
		_autoMapperConfig = autoMapperConfig;
	}

	public async Task<GetAllMilestonesResponse> Handle(GetAllMilestonesQuery request, CancellationToken cancellationToken)
	{
		var milestones = (await _uow
			.GetRepository<Milestone>()
			.AllAsync(cancellationToken))
			.ProjectTo<MilestoneDto>(_autoMapperConfig)
			.ToList();

		return new GetAllMilestonesResponse(milestones);
	}
}

