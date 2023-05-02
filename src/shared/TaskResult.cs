using OneOf;
using OneOf.Types;
using System;

namespace TaskProcessor.Shared
{
	public class ExecTaskResult : TaskResult
	{
		protected ExecTaskResult(OneOf<Success, Error<string>> input)
			: base(input)
		{
		}

		public byte? Step { get; set; }

		public override string ToString() =>
			TryPickT1(out Error<string> error, out _)
				? $"Step: '{Step}°' - Error: {error.Value}"
				: "Task executed Sucessfuly";
	}

	public class TaskResult : OneOfBase<Success, Error<string>>
	{
		protected TaskResult(OneOf<Success, Error<string>> input)
			: base(input)
		{
		}

		public static implicit operator TaskResult(Success _) => new(_);
		public static explicit operator Success(TaskResult _) => _.AsT0;

		public static implicit operator TaskResult(Error<string> _) => new(_);
		public static explicit operator Error<string>(TaskResult _) => _.AsT1;

		public static Success Success => new();
		public static Error<string> Error => new();

		public static TaskResult Failure(Exception ex) => new Error<string>(ex.Message);
	}
}