﻿using FastEndpoints;
using TaskProcessor.Domain.Models;

namespace TaskProcessor.Presentation;
public class MilestoneEndpoint : Endpoint<MilestoneDto>
{
    public override void Configure()
    {
        Post("/api/milestone");
    }

    public override Task HandleAsync(MilestoneDto req, CancellationToken ct)
    {
        return base.HandleAsync(req, ct);
    }
}
