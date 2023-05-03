using OneOf;
using OneOf.Types;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces
{
	public interface ITaskPersistence
	{
		Task<OneOf<TaskMessage, Error<string>>> SaveMessageAsync(TaskMessage message, CancellationToken cancellationToken);
		Task<OneOf<Success, Error>> RemoveMessageAsync(TaskMessage message, CancellationToken cancellationToken);
	}
}