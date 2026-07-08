using Domain.Repositories.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI
{
    public static class InfrastructureDependencies
    {
        extension(IServiceCollection services)
        {
            public void RegisterInfrastructureServices(IConfiguration configuration)
            {
                services.AddDbContext<ToDoDBContext>();

                services.AddScoped<IToDoCategoryRepository, ToDoCategoryRepository>();
                services.AddScoped<IToDoStepRepository, ToDoStepRepository>();
                services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();

                services.AddSingleton(sp =>
                {
                    var dbConfiguration = new DbConfiguration();
                    configuration.GetSection("Database").Bind(dbConfiguration);
                    return dbConfiguration;
                });
            }

        }
    }
}
