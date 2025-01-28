using OPS.Infrastructure;
using OPS.Persistence;
using OPS.Service;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddInfrastructure(builder.Configuration, builder.Environment)
        .AddPersistence()
        .AddService();
}

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Host.AddSerilog(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseInfrastructure(app.Environment);
app.MapHealthChecks("health");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Online Proctoring System")
            .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios)
            .WithDefaultOpenAllTags(true)
            .WithLayout(ScalarLayout.Classic);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
