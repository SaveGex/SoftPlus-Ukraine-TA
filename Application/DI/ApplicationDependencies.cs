using Application.Services;
using Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI
{
    public static class ApplicationDependencies
    {
        extension(IServiceCollection services)
        {
            public void RegisterApplicationServices()
            {
                services.AddScoped<IToDoCategoryService, ToDoCategoryService>();
                services.AddScoped<IToDoStepService, ToDoStepService>();
                services.AddScoped<IToDoTaskService, ToDoTaskService>();
            }
        }
    }
}
