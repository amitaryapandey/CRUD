using System;
using System.Linq;
using QuotesApi.Models;

namespace QuotesApi.Data
{
    public static class SeedData
    {
        public static void Initialize(QuotesDbContext context)
        {
            // Check if data already exists
            if (context.Students.Any())
            {
                return; // Database has been seeded
            }

            // Seed Students
            var students = new Student[]
            {
                new Student
                {
                    Name = "Alice Johnson",
                    Email = "alice.johnson@university.edu",
                    StudentId = "STU001",
                    Major = "Computer Science",
                    Year = 3,
                    Gpa = 3.85m,
                    CreatedAt = DateTime.UtcNow
                },
                new Student
                {
                    Name = "Bob Smith",
                    Email = "bob.smith@university.edu",
                    StudentId = "STU002",
                    Major = "Mathematics",
                    Year = 2,
                    Gpa = 3.65m,
                    CreatedAt = DateTime.UtcNow
                },
                new Student
                {
                    Name = "Carol Williams",
                    Email = "carol.williams@university.edu",
                    StudentId = "STU003",
                    Major = "Physics",
                    Year = 4,
                    Gpa = 3.92m,
                    CreatedAt = DateTime.UtcNow
                },
                new Student
                {
                    Name = "David Brown",
                    Email = "david.brown@university.edu",
                    StudentId = "STU004",
                    Major = "Computer Science",
                    Year = 1,
                    Gpa = 3.45m,
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.Students.AddRange(students);
            context.SaveChanges();

            // Seed Courses
            var courses = new Course[]
            {
                new Course
                {
                    Code = "CS101",
                    Name = "Introduction to Programming",
                    Credits = 3,
                    Instructor = "Dr. Sarah Miller",
                    Semester = "Fall 2024",
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    Code = "CS201",
                    Name = "Data Structures",
                    Credits = 4,
                    Instructor = "Dr. John Davis",
                    Semester = "Fall 2024",
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    Code = "MATH301",
                    Name = "Linear Algebra",
                    Credits = 3,
                    Instructor = "Dr. Emily Wilson",
                    Semester = "Fall 2024",
                    CreatedAt = DateTime.UtcNow
                },
                new Course
                {
                    Code = "PHYS201",
                    Name = "Classical Mechanics",
                    Credits = 4,
                    Instructor = "Dr. Michael Chen",
                    Semester = "Fall 2024",
                    CreatedAt = DateTime.UtcNow
                }
            };
            context.Courses.AddRange(courses);
            context.SaveChanges();

            // Seed Enrollments
            var enrollments = new Enrollment[]
            {
                // Alice's enrollments
                new Enrollment { StudentId = students[0].Id, CourseId = courses[0].Id, Grade = "A", Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) },
                new Enrollment { StudentId = students[0].Id, CourseId = courses[1].Id, Grade = "A", Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) },
                new Enrollment { StudentId = students[0].Id, CourseId = courses[2].Id, Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) },
                
                // Bob's enrollments
                new Enrollment { StudentId = students[1].Id, CourseId = courses[0].Id, Grade = "B", Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) },
                new Enrollment { StudentId = students[1].Id, CourseId = courses[2].Id, Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) },
                
                // Carol's enrollments
                new Enrollment { StudentId = students[2].Id, CourseId = courses[1].Id, Grade = "A", Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) },
                new Enrollment { StudentId = students[2].Id, CourseId = courses[3].Id, Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) },
                
                // David's enrollments
                new Enrollment { StudentId = students[3].Id, CourseId = courses[0].Id, Status = "enrolled", EnrolledAt = DateTime.UtcNow.AddMonths(-3) }
            };
            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();

            // Seed Assignments
            var assignments = new Assignment[]
            {
                // CS101 assignments
                new Assignment
                {
                    CourseId = courses[0].Id,
                    Title = "Hello World Program",
                    Description = "Write your first program that prints 'Hello World'",
                    DueDate = DateTime.UtcNow.AddDays(-30),
                    MaxPoints = 50,
                    CreatedAt = DateTime.UtcNow.AddDays(-45)
                },
                new Assignment
                {
                    CourseId = courses[0].Id,
                    Title = "Variables and Data Types",
                    Description = "Complete exercises on variables and data types",
                    DueDate = DateTime.UtcNow.AddDays(-15),
                    MaxPoints = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Assignment
                {
                    CourseId = courses[0].Id,
                    Title = "Control Flow Assignment",
                    Description = "Implement if-else and loops",
                    DueDate = DateTime.UtcNow.AddDays(7),
                    MaxPoints = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-7)
                },
                
                // CS201 assignments
                new Assignment
                {
                    CourseId = courses[1].Id,
                    Title = "Linked List Implementation",
                    Description = "Implement a singly linked list with basic operations",
                    DueDate = DateTime.UtcNow.AddDays(-20),
                    MaxPoints = 150,
                    CreatedAt = DateTime.UtcNow.AddDays(-35)
                },
                new Assignment
                {
                    CourseId = courses[1].Id,
                    Title = "Binary Search Tree",
                    Description = "Implement BST with insert, delete, and search",
                    DueDate = DateTime.UtcNow.AddDays(14),
                    MaxPoints = 200,
                    CreatedAt = DateTime.UtcNow.AddDays(-7)
                },
                
                // MATH301 assignments
                new Assignment
                {
                    CourseId = courses[2].Id,
                    Title = "Matrix Operations",
                    Description = "Solve problems on matrix multiplication and determinants",
                    DueDate = DateTime.UtcNow.AddDays(-10),
                    MaxPoints = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Assignment
                {
                    CourseId = courses[2].Id,
                    Title = "Eigenvalues and Eigenvectors",
                    Description = "Calculate eigenvalues and eigenvectors for given matrices",
                    DueDate = DateTime.UtcNow.AddDays(10),
                    MaxPoints = 150,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                
                // PHYS201 assignments
                new Assignment
                {
                    CourseId = courses[3].Id,
                    Title = "Newton's Laws Problems",
                    Description = "Solve 10 problems on Newton's laws of motion",
                    DueDate = DateTime.UtcNow.AddDays(5),
                    MaxPoints = 100,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                }
            };
            context.Assignments.AddRange(assignments);
            context.SaveChanges();

            // Seed Submissions
            var submissions = new Submission[]
            {
                // Alice's submissions (excellent student)
                new Submission { AssignmentId = assignments[0].Id, StudentId = students[0].Id, PointsEarned = 50, SubmittedAt = DateTime.UtcNow.AddDays(-29), Status = "graded" },
                new Submission { AssignmentId = assignments[1].Id, StudentId = students[0].Id, PointsEarned = 95, SubmittedAt = DateTime.UtcNow.AddDays(-14), Status = "graded" },
                new Submission { AssignmentId = assignments[3].Id, StudentId = students[0].Id, PointsEarned = 145, SubmittedAt = DateTime.UtcNow.AddDays(-19), Status = "graded" },
                new Submission { AssignmentId = assignments[5].Id, StudentId = students[0].Id, PointsEarned = 92, SubmittedAt = DateTime.UtcNow.AddDays(-9), Status = "graded" },
                
                // Bob's submissions (good student)
                new Submission { AssignmentId = assignments[0].Id, StudentId = students[1].Id, PointsEarned = 45, SubmittedAt = DateTime.UtcNow.AddDays(-28), Status = "graded" },
                new Submission { AssignmentId = assignments[1].Id, StudentId = students[1].Id, PointsEarned = 85, SubmittedAt = DateTime.UtcNow.AddDays(-13), Status = "graded" },
                new Submission { AssignmentId = assignments[5].Id, StudentId = students[1].Id, SubmittedAt = DateTime.UtcNow.AddDays(-8), Status = "submitted" },
                
                // Carol's submissions (excellent student)
                new Submission { AssignmentId = assignments[3].Id, StudentId = students[2].Id, PointsEarned = 148, SubmittedAt = DateTime.UtcNow.AddDays(-18), Status = "graded" },
                
                // David's submissions (struggling student)
                new Submission { AssignmentId = assignments[0].Id, StudentId = students[3].Id, PointsEarned = 35, SubmittedAt = DateTime.UtcNow.AddDays(-25), Status = "graded" }
            };
            context.Submissions.AddRange(submissions);
            context.SaveChanges();
        }
    }
}
