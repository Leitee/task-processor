using TaskProcessor.Domain.Abstractions;
using TaskProcessor.Domain.Entities;

namespace TaskProcessor.Domain.CQRS;

public record CreateMilestoneCommand(string Name, IEnumerable<Goal> Goals) : IRequest<Guid>;

internal class CreateMilestoneHanlder : IRequestHandler<CreateMilestoneCommand, Guid>
{
	private readonly IUnitOfWork _uow;

	public CreateMilestoneHanlder(IUnitOfWork uow)
	{
		_uow = uow;
	}

	public async Task<Guid> Handle(CreateMilestoneCommand request, CancellationToken cancellationToken)
	{
		var newMilestone = new Milestone(request.Name)
			.SetGoals(request.Goals.ToArray());

		var entity = await _uow
			.GetRepository<Milestone>()
			.InsertAsync(newMilestone, cancellationToken);

		await _uow.CommitAsync(cancellationToken);

		return entity.Id;
	}
}
