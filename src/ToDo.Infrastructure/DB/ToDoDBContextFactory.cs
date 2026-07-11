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
            var currentDirectory = Directory.GetCurrentDirectory();

            string apiProjectPath = currentDirectory;

            if (currentDirectory.EndsWith("ToDo.Infrastructure"))
            {
                apiProjectPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", "ToDo.Api"));
            }
            else if (Directory.Exists(Path.Combine(currentDirectory, "src", "ToDo.Api")))
            {
                apiProjectPath = Path.GetFullPath(Path.Combine(currentDirectory, "src", "ToDo.Api"));
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
            return new ToDoDBContext(optionsBuilder.Options);
        }
    }

}
