using FluentAssertions;
using Moq;
using TaskProcessor.Common;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;
using TaskProcessor.UnitTests;

namespace TaskProcessor.UnitTests.Engine;

public class DataSynchronizerTasksEngineDefinitionTest
{
	private Mock<TasksEngineDefinitionBase> _mock;

	public DataSynchronizerTasksEngineDefinitionTest()
	{
		_mock = new Mock<TasksEngineDefinitionBase> { CallBase = true };
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
			task.SetupGet(x => x.MaxRetires).Returns(2);
			task.SetupGet(x => x.IsLastStep).Returns(index == qty - 1);

			result[index] = task.Object;
		}

		return result;
	}

	[Fact]
	public void Should_ReturnFirstTask_When_StepIsDefault()
	{
		_mock
			.Setup(x => x.BuildDefinition())
			.Returns(CreateExecutableTasks(5))
			.Verifiable();

		var sut = _mock.Object;

		var currentStep = new StepTask();
		var controlInstance = currentStep.Clone();

		var isNextTask = sut.TryGetNextStepTask(currentStep, out IExecutableStep nextStepTask);

		isNextTask.Should().BeTrue();

		nextStepTask.Name.Should().Be("Task1");

		currentStep.Should().NotBe(controlInstance);
	}

	[Fact]
	public void Should_ReturnNextTask_When_StepIsCompleted()
	{
		_mock
			.Setup(x => x.BuildDefinition())
			.Returns(CreateExecutableTasks(5))
			.Verifiable();

		var sut = _mock.Object;

		var currentStep = new StepTask();
		currentStep.SetNextTask("Task1");
		currentStep.SetAsCompleted();

		var controlInstance = currentStep.Clone();
		controlInstance.SetNextTask("Task2");

		var isNextTask = sut.TryGetNextStepTask(currentStep, out IExecutableStep nextStepTask);

		isNextTask.Should().BeTrue();

		currentStep.Should().BeEquivalentTo(controlInstance);

		nextStepTask.Name.Should().Be("Task2");
	}


	[Fact]
	public void Should_ReturnSameTask_When_NotMaxFailAttemptsReached()
	{
		_mock
			.Setup(x => x.BuildDefinition())
			.Returns(CreateExecutableTasks(5))
			.Verifiable();

		var sut = _mock.Object;

		var currentStep = new StepTask();
		currentStep.SetNextTask("Task1");
		currentStep.SetFailure("Error 1");

		var controlInstance = currentStep.Clone();

		var isNextTask = sut.TryGetNextStepTask(currentStep, out IExecutableStep nextStepTask);

		isNextTask.Should().BeFalse();

		nextStepTask.Name.Should().Be("Task1");

		currentStep.Should().BeEquivalentTo(controlInstance);
	}

	[Fact]
	public void Should_ReturnFailureTask_When_MaxFailAttemptsReached()
	{
		_mock
			.Setup(x => x.BuildDefinition())
			.Returns(CreateExecutableTasks(5))
			.Verifiable();

		var sut = _mock.Object;

		var currentStep = new StepTask();
		currentStep.SetNextTask("Task1");
		currentStep.SetFailure("Error 1");
		currentStep.SetFailure("Error 2");

		var desiredStep = currentStep.Clone();

		var isNextTask = sut.TryGetNextStepTask(currentStep, out IExecutableStep nextStepTask);

		isNextTask.Should().BeTrue();

		nextStepTask.Name.Should().Be("Task5");

		currentStep.Should().BeEquivalentTo(desiredStep);
	}

	[Fact]
	public void Should_ReturnNoTask_When_NextTaskIsLastOne()
	{
		_mock
			.Setup(x => x.BuildDefinition())
			.Returns(CreateExecutableTasks(5))
			.Verifiable();

		var sut = _mock.Object;

		var currentStep = new StepTask();
		currentStep.SetNextTask("Task5");

		var desiredStep = currentStep.Clone();

		var isNextTask = sut.TryGetNextStepTask(currentStep, out IExecutableStep nextStepTask);

		isNextTask.Should().BeFalse();

		nextStepTask.Should().BeNull();
	}

	[Fact]
	public void Should_TrhowExecutableStepsNotFoundException_When_TaskNotFoundWithValidTaskName()
	{
		_mock
			.Setup(x => x.BuildDefinition())
			.Returns(CreateExecutableTasks(5))
			.Verifiable();

		var sut = _mock.Object;
		var currentStep = new StepTask();
		currentStep.SetNextTask("NotExistentTask");

		var desiredStep = currentStep.Clone();

		Func<bool> act = () => sut.TryGetNextStepTask(currentStep, out IExecutableStep nextStepTask);

		act.Should().ThrowExactly<ExecutableStepsNotFoundException>();
	}
}
