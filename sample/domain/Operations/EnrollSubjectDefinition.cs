using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Domain.Operations
{
	public class EnrollSubjectDefinition : TasksEngineDefinitionBase
	{
		private readonly IEnumerable<IExecutableStep> _steps;

		public const string OPERATION_NAME = nameof(EnrollSubjectDefinition);

		public EnrollSubjectDefinition(IEnumerable<IExecutableStep> steps) => _steps = steps;

		public override IEnumerable<IExecutableStep> BuildDefinition() 
			=> _steps.Where(x => x.OperationName.Equals(OPERATION_NAME));
	}
}
