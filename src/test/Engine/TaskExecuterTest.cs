using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Tests.Engine
{
	public class TaskExecuterTest
    {
        private readonly ITaskExecuter _sut;
        private readonly Mock<ITaskPublisher> _publisher = new Mock<ITaskPublisher>();

        public TaskExecuterTest()
        {
			_publisher
				.Setup(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(TaskResult.Success)
				.Verifiable();

			_sut = new TaskExecuter(_publisher.Object, NullLoggerFactory.Instance);
        }

		[Theory ]
		[InlineData(false, false)]
		[InlineData(false, true )]
		[InlineData(true, false)]
		public async void Should_ReturnError_WhenAnyParamsIsNull(bool param1, bool param2)
		{
			var payload = new Mock<IPayload>();
			TaskMessage taskMessage = param1 ? new Mock<TaskMessage>(payload.Object).Object : null;
			IExecutableStep execTask = param2 ? new Mock<IExecutableStep>().Object : null;

			var act = () => _sut.ExecuteNextOperation(taskMessage, execTask, CancellationToken.None);

			await act.Should().NotThrowAsync();

			act.Invoke()
				.GetAwaiter()
				.GetResult()
				.Should()
				.BeError();
		}

		[Fact]
        public void Should_InvokePublisherOnce_WhenTaskIsNotFinal()
        {
            var payload = new Mock<IPayload>();
            var taskMessage = new Mock<TaskMessage>(payload.Object);

            var execTask = new Mock<IExecutableStep>();
            execTask
                .Setup(x => x.ExecuteAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TaskResult.Success)
                .Verifiable();

            var result = _sut.ExecuteNextOperation(taskMessage.Object, execTask.Object, It.IsAny<CancellationToken>())
				.Result;

            result.Should().BeSuccess();
			_publisher.Verify(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Once);

		}

		[Fact]
		public void ShouldNever_InvokePublisher_WhenTaskIsFinal()
		{
			var payload = new Mock<IPayload>();
			var taskMessage = new Mock<TaskMessage>(payload.Object);

			var execTask = new Mock<IExecutableStep>();
			execTask
				.SetupGet(x => x.IsLastStep)
				.Returns(true);
			execTask
				.Setup(x => x.ExecuteAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(TaskResult.Success)
				.Verifiable();

			var result = _sut.ExecuteNextOperation(taskMessage.Object, execTask.Object, It.IsAny<CancellationToken>())
				.Result;

			result.Should().BeSuccess();
			_publisher.Verify(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Never);

		}
	}
}