using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using TaskProcessor.Common;

namespace TaskProcessor.UnitTests
{
	/// <summary>
	/// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
	/// </summary>
	[DebuggerNonUserCode]
	public class TaskResultAssertions
		: TaskResultAssertions<TaskResultAssertions>
	{
		public TaskResultAssertions(TaskResult value)
			: base(value)
		{
		}
	}

#pragma warning disable CS0659 // Ignore not overriding Object.GetHashCode()
#pragma warning disable CA1065 // Ignore throwing NotSupportedException from Equals
	/// <summary>
	/// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
	/// </summary>
	[DebuggerNonUserCode]
	public class TaskResultAssertions<TAssertions>
		where TAssertions : TaskResultAssertions<TAssertions>
	{
		public TaskResultAssertions(TaskResult value)
		{
			Subject = value;
		}

		/// <summary>
		/// Gets the object which value is being asserted.
		/// </summary>
		public TaskResult Subject { get; }

		/// <summary>
		/// Asserts that the value is <see langword="Error"/>.
		/// </summary>
		/// <param name="because">
		/// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
		/// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
		/// </param>
		/// <param name="becauseArgs">
		/// Zero or more objects to format using the placeholders in <paramref name="because" />.
		/// </param>
		public AndConstraint<TAssertions> BeError(string because = "", params object[] becauseArgs)
		{
			Execute.Assertion
				.ForCondition(Subject.IsT1)
				.BecauseOf(because, becauseArgs)
				.FailWith("Expected {context:taskResult} to be Error{reason}, but found {0}.", Subject);

			return new AndConstraint<TAssertions>((TAssertions)this);
		}

		/// <summary>
		/// Asserts that the value is <see langword="Success"/>.
		/// </summary>
		/// <param name="because">
		/// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
		/// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
		/// </param>
		/// <param name="becauseArgs">
		/// Zero or more objects to format using the placeholders in <paramref name="because" />.
		/// </param>
		public AndConstraint<TAssertions> BeSuccess(string because = "", params object[] becauseArgs)
		{
			Execute.Assertion
				.ForCondition(Subject.IsT0)
				.BecauseOf(because, becauseArgs)
				.FailWith("Expected {context:taskResult} to be Success{reason}, but found {0}.", Subject);

			return new AndConstraint<TAssertions>((TAssertions)this);
		}

		/// <inheritdoc/>
		public override bool Equals(object obj) =>
			throw new NotSupportedException("Equals is not part of Fluent Assertions. Did you mean Be() instead?");
	}

	public static class TaskResultExtension
	{
		/// <summary>
		/// Returns an <see cref="BooleanAssertions"/> object that can be used to assert the
		/// current <see cref="bool"/>.
		/// </summary>
		[Pure]
		public static TaskResultAssertions Should(this TaskResult actualValue)
		{
			return new TaskResultAssertions(actualValue);
		}
	}
}