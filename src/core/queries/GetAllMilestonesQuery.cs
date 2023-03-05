
namespace TaskProcessor.Core;

public record GetAllMilestonesQuery : IRequest<GetAllMilestonesDto>;

internal class GetAllMilestonesHandler : IRequestHandler<GetAllMilestonesQuery, GetAllMilestonesDto>
{    
    public Task<GetAllMilestonesDto> Handle(GetAllMilestonesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

