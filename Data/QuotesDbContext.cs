using Microsoft.EntityFrameworkCore;
using QuotesApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesApi.Data
{
    public class QuotesDbContext : DbContext
    {
        public QuotesDbContext(DbContextOptions<QuotesDbContext> options) : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.StudentId).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.StudentId).IsUnique();
                entity.Property(e => e.Major).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Gpa).HasColumnType("decimal(3,2)");
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Configure Course
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.Code).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Credits).IsRequired();
                entity.Property(e => e.Instructor).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Semester).IsRequired().HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();
            });

            // Configure Enrollment
            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Grade).HasMaxLength(2);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("enrolled");
                entity.Property(e => e.EnrolledAt).IsRequired();

                entity.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Enrollments)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Enrollments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Assignment
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasColumnType("text");
                entity.Property(e => e.DueDate).IsRequired();
                entity.Property(e => e.MaxPoints).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(e => e.Course)
                    .WithMany(c => c.Assignments)
                    .HasForeignKey(e => e.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Submission
            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PointsEarned);
                entity.Property(e => e.SubmittedAt).IsRequired();
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("submitted");

                entity.HasIndex(e => new { e.StudentId, e.AssignmentId }).IsUnique();

                entity.HasOne(e => e.Assignment)
                    .WithMany(a => a.Submissions)
                    .HasForeignKey(e => e.AssignmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Student)
                    .WithMany(s => s.Submissions)
                    .HasForeignKey(e => e.StudentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
