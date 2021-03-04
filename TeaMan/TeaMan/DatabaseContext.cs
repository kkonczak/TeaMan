using Microsoft.EntityFrameworkCore;
using TeaMan.Models;

namespace TeaMan
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<TaskStatus> TaskStatuses { get; set; }
        public DbSet<TaskType> TaskTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlite($"Data Source=database.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>().HasOne(e => e.Calendar)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.CalendarId);

            modelBuilder.Entity<UserTask>().HasOne(e => e.TaskStatus)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.TaskStatusId);

            modelBuilder.Entity<UserTask>().HasOne(e => e.TaskType)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.TaskTypeId);

            modelBuilder.Entity<TaskStatus>().HasOne(e => e.Calendar)
                .WithMany(e => e.TaskStatuses)
                .HasForeignKey(e => e.CalendarId);

            modelBuilder.Entity<TaskType>().HasOne(e => e.Calendar)
                .WithMany(e => e.TaskTypes)
                .HasForeignKey(e => e.CalendarId);
        }
    }
}
