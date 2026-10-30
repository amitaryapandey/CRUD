# Quick Start Guide - Student Dashboard

## Prerequisites
- .NET Core 2.1 SDK or later
- SQL Server LocalDB (included with Visual Studio)

## Getting Started

### 1. Clone and Run
```bash
git clone <repository-url>
cd CRUD
dotnet restore
dotnet run
```

### 2. Access the Dashboard
Open your browser and navigate to:
- **Dashboard UI**: `https://localhost:5001/` or `http://localhost:5000/`
- **API**: `https://localhost:5001/api/students`

### 3. Sample Data
The application automatically seeds sample data on first run:
- 4 Students (Alice, Bob, Carol, David)
- 4 Courses (CS101, CS201, MATH301, PHYS201)
- 8 Enrollments
- 8 Assignments
- 9 Submissions

### 4. Using the Dashboard

#### View Student Dashboard
1. Open the web UI at the root URL
2. Select a student from the dropdown
3. View their:
   - Personal information
   - Statistics (courses, assignments, scores)
   - Enrolled courses with grades
   - Upcoming assignments
   - All assignments with submission status

#### API Examples

**Get all students:**
```bash
curl https://localhost:5001/api/students
```

**Get student dashboard:**
```bash
curl https://localhost:5001/api/students/1/dashboard
```

**Create a new student:**
```bash
curl -X POST https://localhost:5001/api/students \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Jane Doe",
    "email": "jane.doe@university.edu",
    "studentId": "STU005",
    "major": "Engineering",
    "year": 2,
    "gpa": 3.75
  }'
```

**Enroll student in course:**
```bash
curl -X POST https://localhost:5001/api/enrollments \
  -H "Content-Type: application/json" \
  -d '{
    "studentId": 1,
    "courseId": 1
  }'
```

**Submit an assignment:**
```bash
curl -X POST https://localhost:5001/api/submissions \
  -H "Content-Type: application/json" \
  -d '{
    "assignmentId": 1,
    "studentId": 1
  }'
```

**Grade a submission:**
```bash
curl -X PUT https://localhost:5001/api/submissions/1 \
  -H "Content-Type: application/json" \
  -d '{
    "pointsEarned": 95
  }'
```

## API Endpoints Summary

### Students
- `GET /api/students` - List all students
- `GET /api/students/{id}` - Get student details
- `GET /api/students/{id}/dashboard` - Get complete dashboard
- `POST /api/students` - Create student
- `PUT /api/students/{id}` - Update student
- `DELETE /api/students/{id}` - Delete student

### Courses
- `GET /api/courses` - List all courses
- `GET /api/courses/{id}` - Get course details
- `POST /api/courses` - Create course
- `PUT /api/courses/{id}` - Update course
- `DELETE /api/courses/{id}` - Delete course

### Enrollments
- `GET /api/enrollments` - List all enrollments
- `GET /api/enrollments/{id}` - Get enrollment details
- `POST /api/enrollments` - Enroll student
- `PUT /api/enrollments/{id}` - Update enrollment/grade
- `DELETE /api/enrollments/{id}` - Drop enrollment

### Assignments
- `GET /api/assignments` - List all assignments
- `GET /api/assignments/{id}` - Get assignment details
- `POST /api/assignments` - Create assignment
- `PUT /api/assignments/{id}` - Update assignment
- `DELETE /api/assignments/{id}` - Delete assignment

### Submissions
- `GET /api/submissions` - List all submissions
- `GET /api/submissions/{id}` - Get submission details
- `POST /api/submissions` - Submit assignment
- `PUT /api/submissions/{id}` - Update/grade submission
- `DELETE /api/submissions/{id}` - Delete submission

## Dashboard Features

### Statistics Card
- **Total Courses**: Number of courses enrolled
- **Total Assignments**: All assignments across all courses
- **Submitted**: Count of submitted assignments
- **Graded**: Count of graded submissions
- **Average Score**: Percentage score across all graded work

### Enrolled Courses
- Course code and name
- Credits
- Instructor
- Current grade (A, B, C, D, F, or "In Progress")
- Color-coded grade badges

### Upcoming Assignments
- Shows only assignments that are:
  - Not yet submitted
  - Due date is in the future
- Displays course, title, and due date

### All Assignments
- Complete list of assignments from all enrolled courses
- Shows submission status:
  - **Pending** (red) - Not submitted
  - **Submitted** (blue) - Submitted but not graded
  - **Graded** (green) - Graded with score
- Displays score as "X/Y points" for graded work

## Troubleshooting

### Database Issues
If you encounter database errors:
```bash
# Delete the database and restart
rm *.db
dotnet run
```

### Port Already in Use
Edit `Properties/launchSettings.json` to change the port numbers.

### CORS Issues
The application is configured to allow all origins in development. For production, update `Startup.cs` to restrict CORS.

## Next Steps

1. **Customize the UI**: Edit `wwwroot/index.html` to match your branding
2. **Add Authentication**: Implement JWT or cookie-based auth
3. **Add Validation**: Enhance model validation rules
4. **Add Tests**: Create unit and integration tests
5. **Deploy**: Publish to Azure, AWS, or your preferred hosting platform

## Support

For issues or questions, refer to:
- `STUDENT_DASHBOARD.md` - Complete documentation
- `CRUD_OPERATIONS.md` - CRUD operation examples
- `IMPLEMENTATION_SUMMARY.md` - Implementation details
