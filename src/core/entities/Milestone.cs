namespace TaskProcessor.Core;
internal class Milestone : EntityBase
{
    public string Name { get; private set; }
    public virtual List<Goal>? Goals { get; private set; }

    public Milestone(string name) => Name = name;

    public Milestone SetGoals(params Goal[] goals)
    {
        ArgumentNullException.ThrowIfNull(goals);

        Goals!.AddRange(goals);
        return this;
    }
}