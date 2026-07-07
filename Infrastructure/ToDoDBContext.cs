
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class ToDoDBContext : DbContext
    {

        IConfiguration Configuration { get; init; }
        public ToDoDBContext(DbContextOptions<ToDoDBContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<ToDoTask> TodoTasks { get; set; }
        public virtual DbSet<ToDoStep> TodoSteps { get; set; }
        public virtual DbSet<ToDoCategory> TodoCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ToDoTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

                entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Note);

                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
                entity.Property(e => e.IsImportant).HasDefaultValue(false);
                entity.Property(e => e.IsMyDay).HasDefaultValue(false);

                entity.Property(e => e.ReminderAt);
                entity.Property(e => e.DueDate);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.Recurrence).HasDefaultValue(0).HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<RecurrenceType>(v)
                );

                entity.HasOne(task => task.Category)
                    .WithMany(category => category.Tasks)
                    .HasForeignKey(task => task.CategoryId);

                entity.HasMany(task => task.Steps)
                    .WithOne(step => step.TodoTask)
                    .HasForeignKey(step => step.TodoTaskId);
            });
        }
    }
}
