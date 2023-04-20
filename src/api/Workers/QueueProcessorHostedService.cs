using MediatR;
using TaskProcessor.Core.CQRS;
using TaskProcessor.Core.IO;
using TaskProcessor.Core.Shared.engine;

namespace VisionBox.DataSynchronizer.GATEUYSISCAP2.Api.Workers;

public sealed class QueueProcessorHostedService : BackgroundService
{
	private readonly ILogger _logger;
	private readonly ISender _sender;
	private readonly IPubSubHandler _subHandler;

	public QueueProcessorHostedService(ILoggerFactory loggerFactory, ISender sender, IPubSubHandler subHandler)
	{
		_logger = loggerFactory.CreateLogger<QueueProcessorHostedService>();
		_sender = sender;
		_subHandler = subHandler;
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation($"{nameof(QueueProcessorHostedService)} is running");
		return ProcessTaskQueueAsync(stoppingToken);
	}

	private async Task ProcessTaskQueueAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			TaskMessage taskMessage;
			string executableName = string.Empty;

			try
			{
				taskMessage = await _subHandler.ConsumeMessageAsync(stoppingToken);
				executableName = taskMessage.CurrentStep.Name;

				_logger.LogInformation("Dequeuing and sending task '{task}' to be processed", executableName);
				_logger.LogDebug("Task message sending to be processed: '{@msg}'", taskMessage);
				await _sender.Send(new ProcessTaskMessageCommand(taskMessage), stoppingToken);
			}
			catch (OperationCanceledException oex)
			{
				// Prevent throwing if stoppingToken was signaled
				_logger.LogWarning(oex, "Operation was cancelled");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occured when executing [{execution}] task.", executableName);
			}
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation($"{nameof(QueueProcessorHostedService)} is stopping.");
		await base.StopAsync(stoppingToken);
	}
}