using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using OneOf.Types;
using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Tests.Engine
{
    public class TaskDispatcherTest
    {
        private readonly ITaskDispatcher _sut;

        public TaskDispatcherTest()
        {
			var publisher = new Mock<ITaskPublisher>();
			publisher
				.Setup(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new Success())
				.Verifiable();

			_sut = new TaskDispatcher(publisher.Object, NullLoggerFactory.Instance);
        }

        [Fact]
        public void Should_()
        {
            var taskMessage = new Mock<TaskMessage>();
            var execTask = new Mock<IExecutableStep>();

            var result = _sut.DispatchNextOperation(taskMessage.Object, execTask.Object, It.IsAny<CancellationToken>()).Result;

            result.Should().
        }
    }
}