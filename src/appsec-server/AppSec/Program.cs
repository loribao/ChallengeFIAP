using AppSec.Bootstrap;
using AppSec.Domain.Commands;
using AppSec.Domain.Consumers;
using AppSec.Domain.Entities;
using AppSec.Infra;
using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Register.Add(builder.Services, builder.Configuration)
                        .AddMassTransit(x =>
                            {
                                x.AddConsumer<DastStartSubmitConsumer>(cfg =>
                                {
                                    cfg.UseConcurrentMessageLimit(1);
                                });
                                x.AddConsumer<ProjectCreateConsumer>(cfg =>
                                {
                                    cfg.UseConcurrentMessageLimit(1);
                                });
                                   x.AddConsumer<SastSubmitConsumer>(cfg =>
                                {
                                    cfg.UseConcurrentMessageLimit(1);
                                });
                                x.UsingInMemory((context, cfg) =>
                                {
                                    cfg.ConfigureEndpoints(context);
                                });
                            });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/api/project", async (ProjectCreateCommand command, IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(command);
    return Results.Ok(command);
})
  .WithName("Create a new project")
  .WithOpenApi();

app.MapGet("/api/project/", async (ContextAppSec db) =>
{
    var table = await db.Projects.Select(x=>new ProjectEntity{
        Id = x.Id,
        Name = x.Name,
        Description = x.Description,
        CreatedAt = x.CreatedAt,
        Path = x.Path,
        Dast= x.Dast,
        Sast = x.Sast,
        Repository = x.Repository
    }).ToListAsync();
    return Results.Ok(table);
}).WithName("List all projects")
  .WithOpenApi();

app.MapPost("/api/sast/start", async (SastStartCommand command, IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(command);
    return Results.Ok(command);
})
  .WithName("api/sast/start")
  .WithOpenApi();

app.MapPost("/api/dast/start", async (DastStartCommand command, IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(command);
    return Results.Ok(command);
})
.WithName("api/dast/start")
.WithOpenApi();


app.MapGet("/ping", () => "pong")
    .WithName("ping")
    .WithOpenApi();

app.Run();
