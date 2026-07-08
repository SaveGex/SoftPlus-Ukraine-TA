using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DB
{
    internal class ToDoDBContextFactory : IDesignTimeDbContextFactory<ToDoDBContext>
    {
        public ToDoDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ToDoDBContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // for the future
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
            return new ToDoDBContext(optionsBuilder.Options);
        }
    }

}
