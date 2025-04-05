using OPS.Api;
using OPS.Application;
using OPS.Infrastructure;
using OPS.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration, builder.Environment, builder.Host)
    .AddPersistence()
    .AddApplication()
    .AddApi(builder.Configuration);

var app = builder.Build();

app.UseInfrastructure();
app.UseControllers();
app.ApplyMigration();

if (true /*app.Environment.IsDevelopment()*/)
{
    app.UseApiDocumentation();
}

app.Run();