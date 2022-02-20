using Catalogs.Configuration;
using Catalogs.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Net.Mime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    // Fix the async name removal from controller functions
    options.SuppressAsyncSuffixInActionNames = false;
});

var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

ConfigureService();
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

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
    ResponseWriter = async(context,report) =>
    {
        var result = JsonSerializer.Serialize(
            new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(entry => new
                {
                    name = entry.Key,
                    status = entry.Value.Status.ToString(),
                    exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
                    duration = entry.Value.Duration.ToString(),
                })
            });
        context.Response.ContentType = MediaTypeNames.Application.Json;
        await context.Response.WriteAsync(result);
    }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = (_) => false
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();




void ConfigureService()
{
    BsonSerializer.RegisterSerializer(new GuidSerializer(MongoDB.Bson.BsonType.String));
    BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(MongoDB.Bson.BsonType.String));
    builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
    {
        return new MongoClient(mongoDbSettings.ConnectionString);
    });
    builder.Services.AddSingleton<IInMemItemsRepository, MongoDbItemsRepository>();
    builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongoDbSettings.ConnectionString,
        name: "mongodb",
        timeout: TimeSpan.FromSeconds(2),
        tags:new[] {"ready"}
        );
}
