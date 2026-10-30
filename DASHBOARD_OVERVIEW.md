# Student Dashboard - Visual Overview

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         Web Browser                              │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │              Student Dashboard UI (index.html)              │ │
│  │  • Student Selector  • Statistics Cards  • Course Grid     │ │
│  │  • Assignment Lists  • Responsive Design                   │ │
│  └────────────────────────────────────────────────────────────┘ │
└──────────────────────────┬──────────────────────────────────────┘
                           │ HTTP/JSON
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│                    ASP.NET Core 2.1 API                          │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │                      Controllers                          │  │
│  │  • StudentsController    • CoursesController             │  │
│  │  • EnrollmentsController • AssignmentsController         │  │
│  │  • SubmissionsController                                 │  │
│  └──────────────────────────────────────────────────────────┘  │
│                           │                                      │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │                   QuotesDbContext                         │  │
│  │  • Students  • Courses  • Enrollments                    │  │
│  │  • Assignments  • Submissions  • Quotes                  │  │
│  └──────────────────────────────────────────────────────────┘  │
└──────────────────────────┬──────────────────────────────────────┘
                           │ Entity Framework Core
                           ▼
┌─────────────────────────────────────────────────────────────────┐
│                    SQL Server LocalDB                            │
│  Tables: Students, Courses, Enrollments, Assignments,           │
│          Submissions, Quotes                                     │
└─────────────────────────────────────────────────────────────────┘
```

## Data Model Relationships

```
┌──────────────┐
│   Student    │
│──────────────│
│ Id (PK)      │◄────┐
│ Name         │     │
│ Email (UQ)   │     │
│ StudentId(UQ)│     │
│ Major        │     │
│ Year         │     │
│ Gpa          │     │
└──────────────┘     │
       │             │
       │ 1:N         │ 1:N
       │             │
       ▼             │
┌──────────────┐     │         ┌──────────────┐
│ Enrollment   │     │         │  Submission  │
│──────────────│     │         │──────────────│
│ Id (PK)      │     │         │ Id (PK)      │
│ StudentId(FK)├─────┘         │ StudentId(FK)├─────┐
│ CourseId(FK) ├─────┐         │ AssignmentId │     │
│ Grade        │     │         │ PointsEarned │     │
│ Status       │     │         │ SubmittedAt  │     │
└──────────────┘     │         │ Status       │     │
                     │         └──────────────┘     │
                     │                │             │
                     │ N:1            │ N:1         │
                     │                │             │
                     ▼                ▼             │
              ┌──────────────┐ ┌──────────────┐    │
              │    Course    │ │  Assignment  │    │
              │──────────────│ │──────────────│    │
              │ Id (PK)      │ │ Id (PK)      │    │
              │ Code (UQ)    │ │ CourseId(FK) ├────┤
              │ Name         │ │ Title        │    │
              │ Credits      │ │ Description  │    │
              │ Instructor   │ │ DueDate      │    │
              │ Semester     │ │ MaxPoints    │    │
              └──────────────┘ └──────────────┘    │
                     │                              │
                     │ 1:N                          │
                     └──────────────────────────────┘
```

## Dashboard Data Flow

```
User selects student → GET /api/students/{id}/dashboard
                              │
                              ▼
                    ┌─────────────────────┐
                    │ StudentsController  │
                    │   GetDashboard()    │
                    └─────────────────────┘
                              │
                              ▼
                    ┌─────────────────────┐
                    │  Query Database     │
                    │  • Student info     │
                    │  • Enrollments      │
                    │  • Assignments      │
                    │  • Submissions      │
                    └─────────────────────┘
                              │
                              ▼
                    ┌─────────────────────┐
                    │  Build Dashboard    │
                    │  • Courses list     │
                    │  • All assignments  │
                    │  • Statistics       │
                    │  • Upcoming items   │
                    └─────────────────────┘
                              │
                              ▼
                    ┌─────────────────────┐
                    │  Return JSON        │
                    │  DashboardDto       │
                    └─────────────────────┘
                              │
                              ▼
                    ┌─────────────────────┐
                    │  Render in UI       │
                    │  • Update cards     │
                    │  • Display lists    │
                    │  • Show stats       │
                    └─────────────────────┘
```

## UI Layout

```
┌────────────────────────────────────────────────────────────────┐
│                     Student Dashboard                           │
├────────────────────────────────────────────────────────────────┤
│  Select Student: [Alice Johnson ▼]                             │
├────────────────────────────────────────────────────────────────┤
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  👤 Alice Johnson                                        │  │
│  │  📧 alice.johnson@university.edu                         │  │
│  │  🆔 STU001  🎓 Computer Science  📅 Year 3  ⭐ 3.85     │  │
│  └──────────────────────────────────────────────────────────┘  │
├────────────────────────────────────────────────────────────────┤
│  ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐ ┌─────────┐ │
│  │ Courses │ │  Total  │ │Submitted│ │ Graded  │ │ Average │ │
│  │    3    │ │   7     │ │    4    │ │    4    │ │  95.5%  │ │
│  └─────────┘ └─────────┘ └─────────┘ └─────────┘ └─────────┘ │
├────────────────────────────────────────────────────────────────┤
│  📚 Enrolled Courses                                            │
│  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐          │
│  │ CS101        │ │ CS201        │ │ MATH301      │          │
│  │ Intro to Prog│ │ Data Struct  │ │ Linear Alg   │          │
│  │ Dr. Miller   │ │ Dr. Davis    │ │ Dr. Wilson   │          │
│  │ Grade: A     │ │ Grade: A     │ │ In Progress  │          │
│  └──────────────┘ └──────────────┘ └──────────────┘          │
├────────────────────────────────────────────────────────────────┤
│  ⏰ Upcoming Assignments                                        │
│  • CS101 - Control Flow Assignment (Due: Dec 15)               │
│  • CS201 - Binary Search Tree (Due: Dec 22)                    │
│  • MATH301 - Eigenvalues (Due: Dec 18)                         │
├────────────────────────────────────────────────────────────────┤
│  📝 All Assignments                                             │
│  ✅ CS101 - Hello World (Graded: 50/50)                        │
│  ✅ CS101 - Variables (Graded: 95/100)                         │
│  ⏳ CS101 - Control Flow (Pending)                             │
│  ✅ CS201 - Linked List (Graded: 145/150)                      │
│  ⏳ CS201 - Binary Search Tree (Pending)                       │
│  ✅ MATH301 - Matrix Operations (Graded: 92/100)               │
│  ⏳ MATH301 - Eigenvalues (Pending)                            │
└────────────────────────────────────────────────────────────────┘
```

## API Response Example

### GET /api/students/1/dashboard

```json
{
  "student": {
    "id": 1,
    "name": "Alice Johnson",
    "email": "alice.johnson@university.edu",
    "studentId": "STU001",
    "major": "Computer Science",
    "year": 3,
    "gpa": 3.85
  },
  "statistics": {
    "totalCourses": 3,
    "totalAssignments": 7,
    "submittedAssignments": 4,
    "gradedAssignments": 4,
    "averageScore": "95.5"
  },
  "courses": [
    {
      "id": 1,
      "code": "CS101",
      "name": "Introduction to Programming",
      "credits": 3,
      "instructor": "Dr. Sarah Miller",
      "semester": "Fall 2024",
      "grade": "A",
      "status": "enrolled"
    }
  ],
  "assignments": [
    {
      "id": 1,
      "courseId": 1,
      "title": "Hello World Program",
      "dueDate": "2024-11-08T00:00:00Z",
      "maxPoints": 50,
      "courseCode": "CS101",
      "courseName": "Introduction to Programming",
      "pointsEarned": 50,
      "submittedAt": "2024-11-09T00:00:00Z",
      "submissionStatus": "graded"
    }
  ],
  "upcomingAssignments": [
    {
      "id": 3,
      "courseId": 1,
      "title": "Control Flow Assignment",
      "dueDate": "2024-12-15T00:00:00Z",
      "maxPoints": 100,
      "courseCode": "CS101",
      "courseName": "Introduction to Programming",
      "submissionStatus": null
    }
  ]
}
```

## Color Scheme

### Grades
- **A** → 🟢 Green (#28a745)
- **B** → 🔵 Blue (#007bff)
- **C** → 🟡 Yellow (#ffc107)
- **D** → 🟠 Orange (#fd7e14)
- **F** → 🔴 Red (#dc3545)
- **In Progress** → ⚪ Gray (#6c757d)

### Status Badges
- **Pending** → 🔴 Red background
- **Submitted** → 🔵 Blue background
- **Graded** → 🟢 Green background

### UI Theme
- **Primary Gradient** → Purple (#667eea → #764ba2)
- **Cards** → White with shadow
- **Text** → Dark gray (#333)
- **Hover** → Scale transform + shadow

## Key Features Summary

✅ **5 New Models** - Student, Course, Enrollment, Assignment, Submission  
✅ **5 New Controllers** - Full CRUD for each entity  
✅ **Dashboard Endpoint** - Aggregated data with statistics  
✅ **Web UI** - Beautiful responsive interface  
✅ **Sample Data** - 4 students, 4 courses, 8 assignments  
✅ **Relationships** - Foreign keys with cascade delete  
✅ **Constraints** - Unique indexes, required fields  
✅ **Auto-Seeding** - Database populated on startup  
✅ **Documentation** - Complete API reference  
✅ **CORS Enabled** - API accessible from any origin  

## Technology Stack

- **Backend**: ASP.NET Core 2.1
- **ORM**: Entity Framework Core
- **Database**: SQL Server LocalDB
- **Frontend**: HTML5, CSS3, JavaScript (Vanilla)
- **API**: RESTful JSON
- **Architecture**: MVC with Repository Pattern
