using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Operations;

public class DummyWorkflow : TaskEngineDefinitionBase
{
	public const string OPERATION_NAME = nameof(DummyWorkflow);
	public readonly IDateTimeProvider _dateTimeProvider;

	public DummyWorkflow(IEnumerable<IExecutableStep> steps, IDateTimeProvider dateTimeProvider)
		: base(steps)
	{
		_dateTimeProvider = dateTimeProvider;
	}

	public override IEnumerable<IExecutableStep> BuildDefinition(IEnumerable<IExecutableStep> executableSteps)
		=> executableSteps.Where(x => x.OperationName.Equals(OPERATION_NAME));

	public override TaskMessage BuildMessageWithPayload(IPayload payload)
	{
		return new TaskMessage(OPERATION_NAME, payload.CorrelationId, _dateTimeProvider)
			.SetPayload(payload);
	}
}
