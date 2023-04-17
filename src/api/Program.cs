using Serilog;
using TaskProcessor.Core;
using TaskProcessor.Presentation;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCoreApplication(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);
builder.Services.AddSwaggerGen();

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

app.Run();
