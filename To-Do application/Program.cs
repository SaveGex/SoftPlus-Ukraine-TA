using Infrastructure.DI;
using Application.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddConfiguration(new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build());

builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterApplicationServices();

var app = builder.Build();

app.UseRouting();

app.UseHttpsRedirection();

app.UseStaticFiles(); // probably here gonna be the Icons

app.MapControllers();

app.Run();
