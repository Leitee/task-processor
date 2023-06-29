using Microsoft.OpenApi.Models;
using Serilog;
using TaskProcessor.Domain;
using TaskProcessor.Infrastructure;
using VisionBox.DataSynchronizer.GATEUYSISCAP2.Api.Jobs;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "2.0.0",
		Title = "TaskProcessor",
		Description = "A prof of concept for a scalable and reliable task processor",
	});
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHealthChecks();
builder.Services.AddHostedService<TaskConsumerHostedService>();
builder.Services.AddCoreApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger(options => options.SerializeAsV2 = true);
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "DataSynchronizer API V1");
	});
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHealthChecks("/hc");

app.Run();
