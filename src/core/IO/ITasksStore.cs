using TaskProcessor.Core.Shared.engine;
using TaskProcessor.Core.Shared.Interfaces;

namespace TaskProcessor.Core.IO;

public interface ITasksStore : IRepository<TaskMessage, Guid>
{

}