namespace TaskProcessor.Core;
internal class Milestone
{
    public string Name { get; private set; }
    public virtual IEnumerable<Goal>? Goals { get; private set; }

    public Milestone(string name)
    {
        Name = name;
    }
}