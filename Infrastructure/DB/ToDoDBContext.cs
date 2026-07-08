
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DB
{
    internal class ToDoDBContext : DbContext
    {

        IConfiguration? Configuration { get; init; }

        public ToDoDBContext(DbContextOptions<ToDoDBContext> options)
            : base(options)
        {
        }

        public ToDoDBContext(DbContextOptions<ToDoDBContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<ToDoTask> ToDoTasks { get; set; }
        public virtual DbSet<ToDoStep> ToDoSteps { get; set; }
        public virtual DbSet<ToDoCategory> ToDoCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration?.GetConnectionString("DefaultConnection"));
            }
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

                entity.Property(e => e.Recurrence)
                    .HasDefaultValue(RecurrenceType.None)
                    .HasConversion(
                        v => v.ToString(),
                        v => Enum.Parse<RecurrenceType>(v)
                    );

                entity.HasOne(task => task.Category)
                    .WithMany(category => category.Tasks)
                    .HasForeignKey(task => task.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasMany(task => task.Steps)
                    .WithOne(step => step.TodoTask)
                    .HasForeignKey(step => step.TodoTaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ToDoStep>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

                entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
                entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<ToDoCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Icon);
            });
        }
    }
}
