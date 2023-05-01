using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Domain.Entities;
public class Milestone : BaseEntity, IPayload
{
	public string Name { get; private set; }
	public virtual IEnumerable<Goal>? Goals { get; private set; }

	public Milestone(string name)
	{
		Name = name;
	}

	public Milestone SetGoals(IEnumerable<Goal> goals)
	{
		if (goals?.Any() is true)
		{
			Goals = goals;
			return this;
		}

		throw new ArgumentNullException(nameof(goals));
	}

	internal object ToMessage()
	{
		throw new NotImplementedException();
	}
}