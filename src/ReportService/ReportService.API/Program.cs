
using Microsoft.OpenApi.Models;
using ReportService.Application.Event;
using ReportService.Application.Services;
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

builder.Services.AddScoped<IReportService, ReportService.Application.Services.ReportService>();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "patat",
        Title = "Report Service",
        Contact = new OpenApiContact
        {
            Name = "Burak Patat",
            Email = "burak@patat.co",
            Url = new Uri("https://patat.co/")
        },
        Description = "NeredeKal Case Study"
    });
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
        .AllowAnyMethod()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHealthChecks("/health");

app.UseCors("CorsPolicy");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Service");
});

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
