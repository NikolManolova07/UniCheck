using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using UniCheckManagementSystem.Models;

namespace UniCheckManagementSystem.DAL
{
    public class UniCheckDbContext : DbContext
    {
        public UniCheckDbContext() : base("UniCheckContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Lecturers).WithMany(l => l.Courses)
                .Map(t => t.MapLeftKey("CourseId")
                    .MapRightKey("LecturerId")
                    .ToTable("CourseLecturer"));

            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.CourseId, e.StudentId })
                .IsUnique(true);

            modelBuilder.Entity<Student>()
                .HasIndex(s => new { s.FacultyNumber })
                .IsUnique(true);
        }
    }
}