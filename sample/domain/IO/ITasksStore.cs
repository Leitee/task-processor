using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Domain.IO;

public interface ITasksStore : IRepository<TaskMessage, Guid>
{

}