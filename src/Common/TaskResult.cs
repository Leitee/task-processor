using OneOf;
using OneOf.Types;
using System;

namespace TaskProcessor.Common
{
    public class ExecutableStepResult : OneOfBase<InvalidOperationException, TaskResult>
	{
        protected ExecutableStepResult(OneOf<InvalidOperationException, TaskResult> input)
            : base(input)
        {
        }

		public static TaskResult AsSuccess => TaskResult.AsSuccess;
		public static TaskResult AsError => TaskResult.AsError;
        public static InvalidOperationException AsInvalid => new();


		public static implicit operator ExecutableStepResult(InvalidOperationException _) => new(_);
        public static explicit operator InvalidOperationException(ExecutableStepResult _) => _.AsT0;

        public static implicit operator ExecutableStepResult(TaskResult _) => new(_);
        public static explicit operator TaskResult(ExecutableStepResult _) => _.AsT1;

		public byte? Step { get; set; }

		public bool TryPickInvalid(out InvalidOperationException value, out TaskResult remainder)
	        => base.TryPickT0(out value, out remainder);

		public override string ToString() =>
            this.AsT1.TryPickError(out Error<string> error, out _)
                ? $"Step: '{Step}°' - Error: {error.Value}"
                : "Task executed Sucessfuly";
    }

    public class TaskResult : TaskResult<Success>
    {
        protected TaskResult(OneOf<Success, Error<string>> input)
            : base(input)
        {
        }

        public bool IsSuccess => base.IsT0;
        public bool IsError => base.IsT1;

        public static implicit operator TaskResult(Success _) => new(_);
        public static explicit operator Success(TaskResult _) => _.AsT0;

        public static implicit operator TaskResult(Error<string> _) => new(_);
        public static explicit operator Error<string>(TaskResult _) => _.AsT1; 

		public static Success AsSuccess => new();
    }

    [GenerateOneOf]
    public class TaskResult<TResult> : OneOfBase<TResult, Error<string>>
    {
        protected TaskResult(OneOf<TResult, Error<string>> input)
            : base(input)
        {
        }

        public static implicit operator TaskResult<TResult>(TResult _) => new(_);
        public static explicit operator TResult(TaskResult<TResult> _) => _.AsT0;

        public static implicit operator TaskResult<TResult>(Error<string> _) => new(_);
        public static explicit operator Error<string>(TaskResult<TResult> _) => _.AsT1;

		public bool TryPickSuccess(out TResult value, out Error<string> remainder)
	        => base.TryPickT0(out value, out remainder);

		public bool TryPickError(out Error<string> value, out TResult remainder)
	        => base.TryPickT1(out value, out remainder);

		public static Error<string> AsError => new();
        public static Error<string> ErrorFromException(Exception ex) => new(ex.Message);
    }
}