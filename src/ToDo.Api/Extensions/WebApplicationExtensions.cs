using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB.Extensions
{
    public static class WebApplicationExtensions
    {
        extension (WebApplication app)
        {
            public IApplicationBuilder ExecuteMigrations()
            {
                try
                {
                    using var scope = app.Services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<Infrastructure.DB.ToDoDBContext>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "An error occurred while migrating the database.");
                }
                return app;
            }
        }
    }
}
