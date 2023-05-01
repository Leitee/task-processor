namespace TaskProcessor.Domain.Models;

public record StudentDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Grade { get; set; }
}
