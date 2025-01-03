using HotelService.Application;
using HotelService.Application.Services;
using HotelService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using SharedKernel;
using SharedKernel.ElasticSearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//service registiration
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSharedKernel(builder.Configuration, builder.Host);

builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddScoped<IHotelService, HotelService.Application.Services.HotelService>();

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "patat",
        Title = "Hotel Service",
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
        .AllowCredentials().WithOrigins("http://localhost:5284");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Neredekal Case - HotelService")
            .WithTheme(ScalarTheme.Moon)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseCors("CorsPolicy");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    db.Database.Migrate();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Service");
});

app.UseSharedLogging();//elk

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
