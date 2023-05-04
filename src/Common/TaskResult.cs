using OneOf;
using OneOf.Types;
using System;

namespace TaskProcessor.Common
{
    public class TaskResultOrdered : TaskResult
    {
        protected TaskResultOrdered(OneOf<Success, Error<string>> input)
            : base(input)
        {
        }

        public byte? Step { get; set; }

        public override string ToString() =>
            TryPickT1(out Error<string> error, out _)
                ? $"Step: '{Step}°' - Error: {error.Value}"
                : "Task executed Sucessfuly";
    }

    public class TaskResult : TaskResult<Success>
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
    }

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

        public static Error<string> Error => new();
        public static Error<string> ErrorFromException(Exception ex) => new(ex.Message);
    }
}