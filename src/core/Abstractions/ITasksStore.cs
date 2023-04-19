using TaskProcessor.Core.Shared.engine;
using TaskProcessor.Core.Shared.Interfaces;

namespace TaskProcessor.Core.Abstractions;

public interface ITasksStore : IRepository<TaskMessage, Guid>
{

}