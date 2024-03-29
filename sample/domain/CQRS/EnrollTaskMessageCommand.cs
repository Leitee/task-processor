﻿using TaskProcessor.Domain.Entities;
using TaskProcessor.Domain.Models;
using TaskProcessor.Domain.Operations;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.CQRS;

public record EnrollTaskMessageCommand(StudentDto Student) : IRequest;

internal class EnrollTaskMessageCommandHanlder : IRequestHandler<EnrollTaskMessageCommand>
{
	private readonly ILogger _logger;
	private readonly ITaskDispatcher _taskDispatcher;

	public EnrollTaskMessageCommandHanlder(ILoggerFactory loggerFactory, ITaskDispatcher taskDispatcher)
	{
		_logger = loggerFactory.CreateLogger<EnrollTaskMessageCommandHanlder>();
		_taskDispatcher = taskDispatcher;
	}

	public async Task Handle(EnrollTaskMessageCommand request, CancellationToken cancellationToken)
	{
		var student = new Student(request.Student);
		var taskMessage = new TaskMessage(EnrollStudentOperation.OPERATION_NAME).SetPayload(student);
		await _taskDispatcher.DispatchNewOperation(taskMessage, cancellationToken);
	}
}
