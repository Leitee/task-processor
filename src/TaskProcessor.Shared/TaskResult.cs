using OneOf;
using OneOf.Types;
using System.Diagnostics;

namespace TaskProcessor.Shared;

public class ExecTaskResult : TaskResult
{
	protected ExecTaskResult(OneOf<Success, Error<string>> input)
		: base(input)
	{
	}

	public byte? Step { get; set; }

	public override string ToString() =>
		TryPickT1(out Error<string> error, out _) ?
			"Task executed Sucessfuly" :
			$"Step: '{Step}°' - Error: {error.Value}";
}

public class TaskResult : OneOfBase<Success, Error<string>>
{
	protected TaskResult(OneOf<Success, Error<string>> input)
		: base(input)
	{
	}

	public static Success Success => new();
	public static Error<string> Error => new();

	public static implicit operator TaskResult(Success _) => new(_);
	public static explicit operator Success(TaskResult _) => _.AsT0;

	public static implicit operator TaskResult(Error<string> _) => new(_);
	public static explicit operator Error<string>(TaskResult _) => _.AsT1;
}