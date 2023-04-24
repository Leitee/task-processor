using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Core.IO;

public interface ITasksStore : IRepository<TaskMessage, Guid>
{

}