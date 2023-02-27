var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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
