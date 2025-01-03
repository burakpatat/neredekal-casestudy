
using Microsoft.OpenApi.Models;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Ocelot
builder.Configuration.AddJsonFile("ocelot.json");
builder.Services.AddOcelot()
    .AddCacheManager(settings => settings.WithDictionaryHandle());

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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Neredekal CaseStudy Gateway", Version = "patat",
        Contact = new OpenApiContact
        {
            Name = "Burak Patat",
            Email = "burak@patat.co",
            Url = new Uri("https://patat.co/")
        },
        Description = "NeredeKal Case Study"
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Gateway Swagger UI'yi kök dizine yönlendirin
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway");

    // Mikroservislerin Swagger URL'lerini ekleyin
    c.SwaggerEndpoint("http://localhost:5055/swagger/v1/swagger.json", "Hotel API");
    c.SwaggerEndpoint("http://localhost:5232/swagger/v1/swagger.json", "Report API");

    c.RoutePrefix = string.Empty; // Swagger UI'yi kök dizinde aç
});

await app.UseOcelot();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
