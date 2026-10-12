using educoreLMS.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace educoreLMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<Course>()
                .HasMany(c => c.Modules)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.Entity<Lecturer>()
                .HasMany(l => l.Courses)
                .WithOne(c => c.Lecturer)
                .HasForeignKey(c => c.LecturerId)
                .OnDelete(DeleteBehavior.SetNull);

            
            builder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithOne(s => s.Course)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.SetNull);

            
            builder.Entity<Module>()
                .HasMany(m => m.Tasks)
                .WithOne(t => t.Module)
                .HasForeignKey(t => t.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.Entity<Student>()
                .HasIndex(s => s.StudentNumber)
                .IsUnique();

        }
    }
}
