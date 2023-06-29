using AutoMapper;
using TaskProcessor.Domain.IO;
using TaskProcessor.Domain.Models;

namespace TaskProcessor.Domain.CQRS;

public record GetAllMilestonesResponse(IEnumerable<TodoListDto> TodoLists);

public record GetAllMilestonesQuery : IRequest<GetAllMilestonesResponse>;

internal class GetAllMilestonesHandler : IRequestHandler<GetAllMilestonesQuery, GetAllMilestonesResponse>
{
	private readonly ITodoRepository _todoRepository;
	private readonly IConfigurationProvider _autoMapperConfig;

	public GetAllMilestonesHandler(IConfigurationProvider autoMapperConfig, ITodoRepository todoRepository)
	{
		_autoMapperConfig = autoMapperConfig;
		_todoRepository = todoRepository;
	}

	public async Task<GetAllMilestonesResponse> Handle(GetAllMilestonesQuery request, CancellationToken cancellationToken)
	{
		var taskResult = await _todoRepository.GetAllTodoListAsync(cancellationToken);

		if(taskResult.TryPickSuccess(out var todos, out _))
		{
			var todoList = todos.Select(td => new TodoListDto().FromEntity(td));

			return new GetAllMilestonesResponse(todoList);
		}
			
		return new GetAllMilestonesResponse(Enumerable.Empty<TodoListDto>());
	}
}

