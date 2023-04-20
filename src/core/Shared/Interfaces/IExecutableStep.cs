namespace TaskProcessor.Core.Engine;

using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Core.Shared;
using TaskProcessor.Core.Shared.engine;

public interface IExecutableStep
{
    string Name { get; }
    //WorkflowType WorkFlow { get; }
    byte ExecutionOrder { get; }
    byte MaxRetires { get; }
    bool IsLastStep { get; }
    TimeSpan Timeout { get; }
    Task<TaskResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken);
}
