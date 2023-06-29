namespace TaskProcessor.Domain.Entities;
public class TodoItem : BaseEntity
{
	public string Name { get; private set; }
	public string Description { get; private set; }
	public bool Completed { get; private set; }
	public DateTimeOffset Duration { get; private set; }

	public TodoItem(string title, string description, DateTimeOffset duration, bool completed = false)
	{
		Name = title;
		Description = description;
		Completed = completed;
		Duration = duration;
	}

	public void MarkAsCompleted(bool completed = true) => Completed = completed;
}