using Domain.Repositories.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
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
                var dbConfiguration = new DbConfiguration();
                configuration.Bind(dbConfiguration);

                services.AddSingleton(dbConfiguration);

                services.AddDbContext<ToDoDBContext>(options =>
                {
                    options.UseSqlServer(dbConfiguration.DefaultConnection);
                });

                services.AddScoped<IToDoCategoryRepository, ToDoCategoryRepository>();
                services.AddScoped<IToDoStepRepository, ToDoStepRepository>();
                services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();

            }

        }
    }
}
