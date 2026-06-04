using Huquqim.Api.Extensions;
using Huquqim.Api.Middlewares;
using Huquqim.Application;
using Huquqim.Infrastructure;
using Huquqim.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Qatlamlar
builder.Services.ConfigureApiServices();
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureServices(builder.Configuration);

// Global exception handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("Default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Migratsiya + seed (Testing muhitidan tashqari)
if (!app.Environment.IsEnvironment("Testing"))
{
    await app.ApplyMigrationsAsync();
}

app.Run();

public partial class Program;
