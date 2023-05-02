using FluentAssertions;
using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Shared.Tests.Engine
{
	public class TaskMessageTest
	{
		[Fact]
		public void Should_CreateValidMessage_WhenInitialize()
		{
			// Arrange
			var operationName = "TestOperation";

			// Act
			var taskMessage = new TaskMessage(operationName);

			// Assert
			taskMessage.Should().NotBeNull();
			taskMessage.Payload.Should().BeNull();
			taskMessage.Status.Should().Be(MessageStatus.PROCESSING);
			taskMessage.OperationName.Should().Be(operationName);
			taskMessage.CurrentStep.Should().Be(new StepTask());
		}

		[Fact]
		public void Should_BeCompletedTask_WhenMarkedAsCompleted()
		{
			// Arrange
			var operationName = "TestOperation";
			var taskMessage = new TaskMessage(operationName);
			var controlStep = new StepTask();
			controlStep.SetAsCompleted();

			// Act
			taskMessage.MarkCurrentStepAsCompleted();

			// Assert
			taskMessage.Status.Should().Be(MessageStatus.PROCESSING);
			taskMessage.CurrentStep.Should().Be(controlStep);
		}

		[Fact]
		public void Should_BeInCompletedTask_WhenSetError()
		{
			// Arrange
			var operationName = "TestOperation";
			var taskMessage = new TaskMessage(operationName);
			var controlStep = new StepTask();
			controlStep.SetFailure("Error");

			// Act
			taskMessage.SetErrorAtCurrentStep("Error1");

			// Assert
			taskMessage.Status.Should().Be(MessageStatus.PROCESSING);
			taskMessage.CurrentStep.Should().Be(controlStep);
		}

		[Fact]
		public void Should_ReturnPayload_WhenDecoded()
		{
			// Arrange
			var operationName = "TestOperation";
			var payload = new PayloadTest("TestPayload");
			var taskMessage = new TaskMessage(operationName).SetPayload(payload);

			// Act
			var payloadResutl = taskMessage.GetPayload<PayloadTest>();

			// Assert
			taskMessage.Status.Should().Be(MessageStatus.PROCESSING);
			payloadResutl.Should().Be(payload);
			payloadResutl.Should().NotBeSameAs(payload);
		}
	}

	record PayloadTest(string Value) : IPayload;
}
