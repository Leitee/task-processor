using Moq;
using TaskProcessor.Common;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.UnitTests.Engine;

public class TaskDispatcherTest
{
	private readonly Mock<ITaskPersistence> _taskPersistence = new Mock<ITaskPersistence>();
	private readonly Mock<ITaskPublisher> _taskPublisher = new Mock<ITaskPublisher>();
	private readonly TaskMessage _taskMessage = new TaskMessage("TestOperation");

	[Fact]
	public async void Should_DispatchNewOperation_WhenAllValid()
	{
		// Arrange
		_taskPersistence
			.Setup(x => x.SaveMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(_taskMessage)
			.Verifiable();

		_taskPublisher
			.Setup(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(TaskResult.Success)
			.Verifiable();

		var taskDispatcher = new TaskDispatcher(_taskPersistence.Object, _taskPublisher.Object);

		// Act
		var result = await taskDispatcher.DispatchNewOperation(_taskMessage, CancellationToken.None);

		// Assert
		result.Should().BeSuccess();
		_taskPersistence.Verify(x => x.SaveMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Once);
		_taskPublisher.Verify(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async void Should_ReturnTaskError_WhenPublishReturnError()
	{
		// Arrange
		_taskPersistence
			.Setup(x => x.SaveMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(_taskMessage)
			.Verifiable();

		_taskPublisher
			.Setup(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(TaskResult.Error)
			.Verifiable();

		var taskDispatcher = new TaskDispatcher(_taskPersistence.Object, _taskPublisher.Object);

		// Act
		var result = await taskDispatcher.DispatchNewOperation(_taskMessage, CancellationToken.None);

		// Assert
		result.Should().BeError();
		_taskPersistence.Verify(x => x.SaveMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Once);
		_taskPublisher.Verify(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Once);
	}

	[Fact]
	public async void Should_ReturnTaskError_WhenSaveReturnError()
	{
		// Arrange
		_taskPersistence
			.Setup(x => x.SaveMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(TaskResult.Error)
			.Verifiable();

		_taskPublisher
			.Setup(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(TaskResult.Success)
			.Verifiable();

		var taskDispatcher = new TaskDispatcher(_taskPersistence.Object, _taskPublisher.Object);

		// Act
		var result = await taskDispatcher.DispatchNewOperation(_taskMessage, CancellationToken.None);

		// Assert
		result.Should().BeError();
		_taskPersistence.Verify(x => x.SaveMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Once);
		_taskPublisher.Verify(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Never);
	}
}
