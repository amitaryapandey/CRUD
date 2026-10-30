# Student Dashboard

A comprehensive student management system with dashboard functionality for tracking students, courses, enrollments, assignments, and submissions.

## Features

### 1. Student Management
- Full CRUD operations for students
- Track student information: name, email, student ID, major, year, GPA
- Unique constraints on email and student ID

### 2. Course Management
- Full CRUD operations for courses
- Track course code, name, credits, instructor, and semester
- Unique constraint on course code

### 3. Enrollment System
- Enroll students in courses
- Track enrollment status and grades
- Prevent duplicate enrollments (unique constraint on student-course pair)

### 4. Assignment Management
- Create assignments for courses
- Track title, description, due date, and max points
- Link assignments to specific courses

### 5. Submission Tracking
- Students can submit assignments
- Track submission status (submitted, graded)
- Record points earned
- Prevent duplicate submissions (unique constraint on student-assignment pair)

### 6. Student Dashboard
- Comprehensive dashboard view for each student
- Real-time statistics:
  - Total courses enrolled
  - Total assignments
  - Submitted assignments count
  - Graded assignments count
  - Average score percentage
- View all enrolled courses with grades
- View all assignments with submission status
- View upcoming assignments (not submitted, due in future)

## API Endpoints

### Students
- `GET /api/students` - Get all students
- `GET /api/students/{id}` - Get student by ID
- `GET /api/students/{id}/dashboard` - Get complete dashboard data for a student
- `POST /api/students` - Create a new student
- `PUT /api/students/{id}` - Update a student
- `DELETE /api/students/{id}` - Delete a student

### Courses
- `GET /api/courses` - Get all courses
- `GET /api/courses/{id}` - Get course by ID
- `POST /api/courses` - Create a new course
- `PUT /api/courses/{id}` - Update a course
- `DELETE /api/courses/{id}` - Delete a course

### Enrollments
- `GET /api/enrollments` - Get all enrollments
- `GET /api/enrollments/{id}` - Get enrollment by ID
- `POST /api/enrollments` - Enroll a student in a course
- `PUT /api/enrollments/{id}` - Update enrollment (e.g., assign grade)
- `DELETE /api/enrollments/{id}` - Delete enrollment

### Assignments
- `GET /api/assignments` - Get all assignments with course info
- `GET /api/assignments/{id}` - Get assignment by ID
- `POST /api/assignments` - Create a new assignment
- `PUT /api/assignments/{id}` - Update an assignment
- `DELETE /api/assignments/{id}` - Delete an assignment

### Submissions
- `GET /api/submissions` - Get all submissions
- `GET /api/submissions/{id}` - Get submission by ID
- `POST /api/submissions` - Submit an assignment
- `PUT /api/submissions/{id}` - Update submission (e.g., grade it)
- `DELETE /api/submissions/{id}` - Delete submission

## Dashboard UI

A beautiful, responsive web interface is available at the root URL (`/`) that provides:

- **Student Selector**: Dropdown to select any student
- **Student Information Card**: Displays student details (name, ID, email, major, year, GPA)
- **Statistics Cards**: Visual display of key metrics
- **Enrolled Courses**: Grid view of all courses with grades
- **Upcoming Assignments**: List of pending assignments due in the future
- **All Assignments**: Complete list with submission status and scores

### UI Features
- Modern gradient design with purple theme
- Responsive grid layouts
- Color-coded grade badges (A-F)
- Status badges for assignments (Pending, Submitted, Graded)
- Hover effects and smooth transitions
- Real-time data loading from API

## Data Models

### Student
```csharp
{
    Id: int,
    Name: string (required, max 255),
    Email: string (required, max 255, unique),
    StudentId: string (required, max 50, unique),
    Major: string (required, max 100),
    Year: int (required),
    Gpa: decimal? (3,2),
    CreatedAt: DateTime
}
```

### Course
```csharp
{
    Id: int,
    Code: string (required, max 20, unique),
    Name: string (required, max 255),
    Credits: int (required),
    Instructor: string (required, max 255),
    Semester: string (required, max 50),
    CreatedAt: DateTime
}
```

### Enrollment
```csharp
{
    Id: int,
    StudentId: int (required, FK),
    CourseId: int (required, FK),
    Grade: string (max 2),
    Status: string (required, max 20, default "enrolled"),
    EnrolledAt: DateTime
}
```

### Assignment
```csharp
{
    Id: int,
    CourseId: int (required, FK),
    Title: string (required, max 255),
    Description: string (text),
    DueDate: DateTime (required),
    MaxPoints: int (required),
    CreatedAt: DateTime
}
```

### Submission
```csharp
{
    Id: int,
    AssignmentId: int (required, FK),
    StudentId: int (required, FK),
    PointsEarned: int?,
    SubmittedAt: DateTime,
    Status: string (required, max 20, default "submitted")
}
```

## Database Schema

The application uses Entity Framework Core with SQL Server LocalDB. The database is automatically created on first run with the following relationships:

- **Students** ↔ **Enrollments** (One-to-Many)
- **Courses** ↔ **Enrollments** (One-to-Many)
- **Courses** ↔ **Assignments** (One-to-Many)
- **Students** ↔ **Submissions** (One-to-Many)
- **Assignments** ↔ **Submissions** (One-to-Many)

All foreign key relationships use cascade delete.

## Usage Example

### 1. Create a Student
```bash
POST /api/students
{
    "name": "John Doe",
    "email": "john.doe@university.edu",
    "studentId": "STU001",
    "major": "Computer Science",
    "year": 3,
    "gpa": 3.85
}
```

### 2. Create a Course
```bash
POST /api/courses
{
    "code": "CS101",
    "name": "Introduction to Programming",
    "credits": 3,
    "instructor": "Dr. Smith",
    "semester": "Fall 2024"
}
```

### 3. Enroll Student in Course
```bash
POST /api/enrollments
{
    "studentId": 1,
    "courseId": 1
}
```

### 4. Create an Assignment
```bash
POST /api/assignments
{
    "courseId": 1,
    "title": "Homework 1",
    "description": "Complete exercises 1-10",
    "dueDate": "2024-12-31T23:59:59Z",
    "maxPoints": 100
}
```

### 5. Submit Assignment
```bash
POST /api/submissions
{
    "assignmentId": 1,
    "studentId": 1
}
```

### 6. Grade Submission
```bash
PUT /api/submissions/1
{
    "pointsEarned": 95
}
```

### 7. View Student Dashboard
```bash
GET /api/students/1/dashboard
```

Returns complete dashboard data including student info, courses, assignments, statistics, and upcoming assignments.

## Running the Application

1. Ensure SQL Server LocalDB is installed
2. Run the application:
   ```bash
   dotnet run
   ```
3. Open browser to `https://localhost:5001` (or the configured port)
4. The database will be created automatically on first run
5. Use the web UI to select students and view their dashboards
6. Use the API endpoints to manage data

## Technical Details

- **Framework**: ASP.NET Core 2.1
- **ORM**: Entity Framework Core
- **Database**: SQL Server LocalDB
- **Architecture**: MVC with Repository pattern via DbContext
- **CORS**: Enabled for all origins (development)
- **Static Files**: Enabled for serving the dashboard UI

## Future Enhancements

- Authentication and authorization
- File upload for assignment submissions
- Email notifications for upcoming assignments
- Grade calculation and GPA updates
- Student performance analytics
- Export data to PDF/Excel
- Mobile-responsive improvements
- Dark mode theme
