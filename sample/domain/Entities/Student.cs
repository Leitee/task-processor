using TaskProcessor.Domain.Models;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Entities;

public class Student : BaseEntity, IPayload
{
	public string Name { get; set; } = string.Empty;
	public int Age { get; set; }
	public int Grade { get; set; }

	public Student(string name, int age, int grade)
	{
		Name = name;
		Age = age;
		Grade = grade;
	}

    public Student(StudentDto studentDto) :
		this(studentDto.Name, studentDto.Age, studentDto.Grade)
    {

    }
}
