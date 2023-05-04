using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Operations;

public class EnrollStudentOperation : TasksEngineDefinitionBase
{
	private readonly IEnumerable<IExecutableStep> _steps;

	public const string OPERATION_NAME = nameof(EnrollStudentOperation);

	public EnrollStudentOperation(IEnumerable<IExecutableStep> steps) => _steps = steps;

	public override IEnumerable<IExecutableStep> BuildDefinition() 
		=> _steps.Where(x => x.OperationName.Equals(OPERATION_NAME));
}
