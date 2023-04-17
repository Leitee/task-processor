using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace TaskProcessor.Presentation;
public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
		services.AddEndpointsApiExplorer();
		return services;
    }


	public static IApplicationBuilder UsePresentation(this IApplicationBuilder app)
    {


		var group = ((WebApplication)app).MapGroup("task");

		group.MapGet("/", () =>
		{
			return new[]
			{
				"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
			};
		})
		.WithName("GetTasks");

		group.MapPost("/", () =>
		{
			return Results.Created("GetTasks", new { id = 1 });
		});

		group.MapDelete("/", () =>
		{
			return Results.Created("GetTasks", new { id = 1 });
		});

		return app;
    }
}