using EventBus;
using ReportService.Application.Event;
using ReportService.Infrastructure.Persistence;
using SharedKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// RabbitMQ and EventBus
builder.Services.AddSharedKernel(builder.Configuration);

// Hosted Services / Event Handlers
builder.Services.AddSingleton<IHostedService, ReportServiceEventListener>();
builder.Services.AddScoped<ReportRequestedEventHandler>();

// Configuration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
