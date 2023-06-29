using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;
using TaskProcessor.UnitTests;
using Xunit;

namespace TaskProcessor.UnitTests.Engine
{
	public class TaskExecuterTest
	{
		private readonly ITaskExecuter _sut;
		private readonly Mock<ITaskPublisher> _publisher = new Mock<ITaskPublisher>();

		public TaskExecuterTest()
		{
			_publisher
				.Setup(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(TaskResult.AsSuccess)
				.Verifiable();

			_sut = new TaskExecuter(NullLoggerFactory.Instance);
		}

		[Theory]
		[InlineData(false, false)]
		[InlineData(false, true)]
		[InlineData(true, false)]
		public async void Should_ReturnError_WhenAnyParamsIsNull(bool param1, bool param2)
		{
			var payload = new Mock<IPayload>();
			TaskMessage taskMessage = param1 ? new Mock<TaskMessage>("Operation1", Guid.NewGuid()).Object : null;
			IExecutableStep execTask = param2 ? new Mock<IExecutableStep>().Object : null;

			Func<Task<TaskResult>> act = () => _sut.ExecuteNextOperation(taskMessage, execTask, CancellationToken.None);

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
			var taskMessage = new TaskMessage("Operation1", Guid.NewGuid());

			var execTask = new Mock<IExecutableStep>();
			execTask
				.Setup(x => x.ExecuteAsync(taskMessage, It.IsAny<CancellationToken>()))
				.ReturnsAsync(ExecutableStepResult.AsSuccess)
				.Verifiable();

			var result = _sut.ExecuteNextOperation(taskMessage, execTask.Object, It.IsAny<CancellationToken>())
			.Result;

			result.Should().BeSuccess();
		}

		[Fact]
		public void Should_SetInvalidAndReturnErrorAndNotInvokePublisher_WhenTaskIsInvalid()
		{
			var taskMessage = new Mock<TaskMessage>("Operation1", Guid.NewGuid());

			var execTask = new Mock<IExecutableStep>();
			execTask
				.Setup(x => x.ExecuteAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(ExecutableStepResult.AsInvalid)
				.Verifiable();

			var result = _sut.ExecuteNextOperation(taskMessage.Object, execTask.Object, It.IsAny<CancellationToken>())
				.Result;

			result.Should().BeError();
			taskMessage.Object.Status.Should().Be(MessageStatus.DEAD_LETTERED);
			_publisher.Verify(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Never);

		}

		[Fact]
		public void ShouldNever_InvokePublisher_WhenTaskIsFinal()
		{
			var payload = new Mock<IPayload>();
			var taskMessage = new TaskMessage("Operation1", Guid.NewGuid());

			var execTask = new Mock<IExecutableStep>();
			execTask
				.SetupGet(x => x.IsLastStep)
				.Returns(true);
			execTask
				.Setup(x => x.ExecuteAsync(taskMessage, It.IsAny<CancellationToken>()))
				.ReturnsAsync(ExecutableStepResult.AsSuccess)
				.Verifiable();

			var result = _sut.ExecuteNextOperation(taskMessage, execTask.Object, It.IsAny<CancellationToken>())
				.Result;

			result.Should().BeSuccess();
			_publisher.Verify(x => x.PublishMessageAsync(It.IsAny<TaskMessage>(), It.IsAny<CancellationToken>()), Times.Never);

		}
	}
}