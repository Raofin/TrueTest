using OPS.Api;
using OPS.Application;
using OPS.Infrastructure;
using OPS.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration, builder.Environment, builder.Host)
    .AddPersistence()
    .AddApplication()
    .AddController();

var app = builder.Build();

app.UseInfrastructure();
app.UseControllers();

app.ApplyMigration();

if (app.Environment.IsDevelopment()) app.UseScalar();

app.Run();