using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using TaskProcessor.Common;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;
using TaskProcessor.UnitTests;
using Xunit;

namespace TaskProcessor.UnitTests.Engine
{
	public class DataSynchronizerTasksEngineDefinitionTest
	{
		private Mock<TaskEngineDefinitionBase> _mock;

		public DataSynchronizerTasksEngineDefinitionTest()
		{
			var deadLetter = new Mock<IFailureStep>();
			deadLetter.SetupGet(x => x.IsDeadLetter).Returns(true);

			var failure = new Mock<IFailureStep>();
			deadLetter.SetupGet(x => x.IsDeadLetter).Returns(false);

			var failureTasks = new List<IExecutableStep> 
			{
				deadLetter.As<IExecutableStep>().Object, 
				failure.As<IExecutableStep>().Object
			};

			_mock = new Mock<TaskEngineDefinitionBase>(failureTasks) { CallBase = true };

			_mock
				.Setup(x => x.BuildDefinition(null))
				.Returns(CreateExecutableTasks(5));
		}

		private static IEnumerable<IExecutableStep> CreateExecutableTasks(int qty)
		{
			IExecutableStep[] result = new IExecutableStep[qty];

			for (int index = 0; index < qty; index++)
			{
				byte order = (byte)(index + 1);
				var task = new Mock<IExecutableStep>();
				task.SetupGet(x => x.ExecutionOrder).Returns(order);
				task.SetupGet(x => x.Name).Returns($"Task{order}");
				task.SetupGet(x => x.MaxRetries).Returns(2);
				task.SetupGet(x => x.IsLastStep).Returns(index == qty - 1);

				result[index] = task.Object;
			}

			return result;
		}

		[Fact]
		public void Should_ReturnFirstTask_When_StepIsDefault()
		{
			_mock
				.Setup(x => x.BuildDefinition(It.IsAny<IEnumerable<IExecutableStep>>()))
				.Returns(CreateExecutableTasks(5))
				.Verifiable();

			var sut = _mock.Object;

			var controlInstance = new StepTask();

			var taskMessage = new TaskMessage("TestOperation", Guid.NewGuid());

			var isNextTask = sut.TryGetNextStepTask(taskMessage, out IExecutableStep nextStepTask);

			isNextTask.Should().BeTrue();

			nextStepTask.Name.Should().Be("Task1");

			taskMessage.CurrentStep.Should().NotBe(controlInstance);
		}

		[Fact]
		public void Should_ReturnNextTask_When_StepIsCompleted()
		{
			_mock
				.Setup(x => x.BuildDefinition(It.IsAny<IEnumerable<IExecutableStep>>()))
				.Returns(CreateExecutableTasks(5))
				.Verifiable();

			var sut = _mock.Object;

			var currentStep = new StepTask();
			currentStep.SetNextTask("Task1");
			currentStep.SetAsCompleted();

			var controlInstance = currentStep.Clone();
			controlInstance.SetNextTask("Task2");

			var taskMessage = new Mock<TaskMessage>("TestOperation", Guid.NewGuid());
			taskMessage.SetupGet(x => x.CurrentStep).Returns(currentStep);

			var isNextTask = sut.TryGetNextStepTask(taskMessage.Object, out IExecutableStep nextStepTask);

			isNextTask.Should().BeTrue();

			currentStep.Should().BeEquivalentTo(controlInstance);

			nextStepTask.Name.Should().Be("Task2");
		}


		[Fact]
		public void Should_ReturnSameTask_When_TaskFailed()
		{
			_mock
				.Setup(x => x.BuildDefinition(It.IsAny<IEnumerable<IExecutableStep>>()))
				.Returns(CreateExecutableTasks(5))
				.Verifiable();

			var sut = _mock.Object;

			var currentStep = new StepTask();
			currentStep.SetNextTask("Task1");
			currentStep.SetFailure("Error 1");

			var controlInstance = currentStep.Clone();

			var taskMessage = new Mock<TaskMessage>("TestOperation", Guid.NewGuid());
			taskMessage.SetupGet(x => x.CurrentStep).Returns(currentStep);

			var isNextTask = sut.TryGetNextStepTask(taskMessage.Object, out IExecutableStep nextStepTask);

			isNextTask.Should().BeTrue();

			nextStepTask.Name.Should().Be("Task1");

			currentStep.Should().BeEquivalentTo(controlInstance);
		}

		[Fact]
		public void Should_ThrowExecutableStepsNotFoundException_When_TaskNotFoundWithValidTaskName()
		{
			_mock
				.Setup(x => x.BuildDefinition(It.IsAny<IEnumerable<IExecutableStep>>()))
				.Returns(CreateExecutableTasks(5))
				.Verifiable();

			var sut = _mock.Object;
			var currentStep = new StepTask();
			currentStep.SetNextTask("NotExistentTask");

			var desiredStep = currentStep.Clone();

			var taskMessage = new Mock<TaskMessage>("TestOperation", Guid.NewGuid());
			taskMessage.SetupGet(x => x.CurrentStep).Returns(currentStep);

			Func<bool> act = () => sut.TryGetNextStepTask(taskMessage.Object, out IExecutableStep nextStepTask);

			act.Should().ThrowExactly<ExecutableStepsNotFoundException>();
		}
	}
}