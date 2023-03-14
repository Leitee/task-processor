using Serilog;
using TaskProcessor.Core;
using TaskProcessor.Infrastructure;
using TaskProcessor.Presentation;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCoreApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UsePresentation();

var group = app.MapGroup("task");

group.MapGet("/", () =>
{    
    return new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };  
})
.WithName("GetTasks")
.WithOpenApi();

group.MapPost("/", () => 
{
    return Results.Created("GetTasks", new {id= 1});
});

group.MapDelete("/", () => 
{
    return Results.Created("GetTasks", new {id= 1});
});

app.Run();
