using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

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
