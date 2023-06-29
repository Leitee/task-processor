using Castle.DynamicProxy;
using FluentAssertions;
using Moq;
using System;
using TaskProcessor.Common;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;
using Xunit;

namespace TaskProcessor.UnitTests.Engine
{
	public class TaskMessageTest
	{
		[Theory]
		[InlineData(null)]
		[InlineData("")]
		public void Should_ThrowArgumentException_WhenOperationNameIsInvalid(string param)
		{
			// Arrange
			var operationName = param;

			// Act
			var action = new Action(() => new TaskMessage(operationName));

			// Assert
			action.Should().Throw<ArgumentException>();
		}

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
		public void Should_BeCompletedStep_WhenMarkedAsCompleted()
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
		public void Should_BeCompletedTask_WhenMarkedAsCompleted()
		{
			// Arrange
			var operationName = "TestOperation";
			var taskMessage = new TaskMessage(operationName);
			var controlStep = new StepTask();

			// Act
			taskMessage.MarkTaskAsCompleted();

			// Assert
			taskMessage.Status.Should().Be(MessageStatus.COMPLETED);
			taskMessage.CurrentStep.Should().Be(controlStep);
		}

		[Fact]
		public void Should_BeInvalidTask_WhenMarkedAsInvalid()
		{
			// Arrange
			var operationName = "TestOperation";
			var taskMessage = new TaskMessage(operationName);

			// Act
			taskMessage.MarkCurrentTaskAsInvalid();

			// Assert
			taskMessage.Status.Should().Be(MessageStatus.DEAD_LETTERED);
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

		[Fact]
		public void Should_BeValidDateTimeProvider_WhenSet()
		{
			// Arrange
			var operationName = "TestOperation";
			var referenceDateTime = DateTimeOffset.UtcNow;
			var dateTimeProvider = new Mock<IDateTimeProvider>();
			dateTimeProvider.Setup(x => x.UtcNow()).Returns(referenceDateTime);

			// Act
			var taskMessage = new TaskMessage(operationName, Guid.NewGuid(), dateTimeProvider.Object);

			// Assert
			taskMessage.CreationDate.Should().Be(referenceDateTime);
		}
	}

	record PayloadTest(string Name) : IPayload
	{
        public Guid CorrelationId { get; set; }
	}
}