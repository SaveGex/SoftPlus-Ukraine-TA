using Domain.Interfaces;
using Infrastructure.Configurations;
using Infrastructure.DB;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
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
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing")
                    {
                        options.UseInMemoryDatabase("TestingDb");
                    }
                    else
                    {
                        options.UseSqlServer(dbConfiguration.DefaultConnection
                            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.")
                        );
                    }
                })
                .AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ToDoDBContext>()
                .AddDefaultTokenProviders();


                services.AddScoped<IToDoCategoryRepository, ToDoCategoryRepository>();
                services.AddScoped<IToDoStepRepository, ToDoStepRepository>();
                services.AddScoped<IToDoTaskRepository, ToDoTaskRepository>();
                services.AddScoped<IModelRepository, ModelRepository>();
                services.AddScoped<IAssetsRepository, AssetsRepository>();
                services.AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>();
            }

        }
    }
}
