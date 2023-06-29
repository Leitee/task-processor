using System.Threading.Channels;
using TaskProcessor.Common;
using TaskProcessor.Domain.IO;
using TaskProcessor.Engine;

namespace TaskProcessor.Infrastructure.Messages;

public class InMemoryPubSubHandler : IPubSubHandler
{
    private readonly Channel<TaskMessage> _queue;

    public InMemoryPubSubHandler()
    {
        BoundedChannelOptions options = new(100)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        _queue = Channel.CreateBounded<TaskMessage>(options);
    }

    public async Task<TaskMessage> ConsumeMessageAsync(CancellationToken cancellationToken)
    {
        var workItem = await _queue.Reader.ReadAsync(cancellationToken);

        return workItem;
    }

    public async Task<TaskResult> PublishMessageAsync(TaskMessage taskMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            await _queue.Writer.WriteAsync(taskMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            return TaskResult.ErrorFromException(ex);
        }

        return TaskResult.AsSuccess;
    }
}
