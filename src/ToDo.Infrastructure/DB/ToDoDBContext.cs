
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB
{
    internal class ToDoDBContext : IdentityDbContext<IdentityUser>
    {
        public ToDoDBContext(DbContextOptions<ToDoDBContext> options)
            : base(options)
        { }

        public virtual DbSet<ToDoTask> ToDoTasks { get; set; }
        public virtual DbSet<ToDoStep> ToDoSteps { get; set; }
        public virtual DbSet<ToDoCategory> ToDoCategories { get; set; }
        public virtual DbSet<Icon> Icons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Fluent API
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

                entity.HasOne(category => category.Icon)
                    .WithMany()
                    .HasForeignKey(category => category.IconId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Icon>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            });
            #endregion
        }
    }
}
