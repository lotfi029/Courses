# Interactive Learning System

## Overview
The Interactive Learning System is a robust and scalable platform designed to provide users with an engaging and structured learning experience. Built using modern technologies, the platform supports course navigation, user progress tracking, and personalized learning pathways.

---

## Features

### 1. Dynamic Course Navigation
- Structured lesson order ensures users follow a logical learning path.
- Automatically directs users to the next lesson upon completion.

### 2. User Progress Tracking
- Tracks completed lessons, timestamps, and interaction history.
- Provides clear and detailed insights into user progress.

### 3. Custom Responses Based on User Status
- Tailored responses and content access for unauthenticated users, authenticated users, and enrolled users.

### 4. Rich Course Structure
- Courses are organized with relationships between lessons, modules, categories, and tags.
- Flexible and detailed database design ensures scalability.

### 5. Mobile-Friendly and Responsive
- Optimized for both desktop and mobile users for seamless access.

### 6. Scalability
- The platform is designed to accommodate additional features, such as quizzes, certificates, and instructor dashboards.

---

## Technical Details

### Backend Technologies
- **Framework**: .NET Core
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **API Design**: RESTful services

### Key Functionalities
- **Lesson Order System**: Ensures smooth navigation through lessons.
- **Progress Updates**: Tracks and updates user progress in real-time.
- **Data Integration**: Utilizes relational database models for complex content structures (e.g., courses, modules, lessons, user-course interactions).

---

## API Endpoints

### 1. Get All Courses
**Endpoint**: `/api/courses`
- **Description**: Retrieves a list of all available courses.
- **Response**: Course details, tags, categories, and enrollment status (if applicable).

### 2. Get User Lessons
**Endpoint**: `/api/user-lessons`
- **Description**: Retrieves lessons for a user based on course, user ID, and progress.
- **Response**: Lesson details, completion status, and next lesson.

### 3. Enroll in Course
**Endpoint**: `/api/enroll`
- **Description**: Enrolls an authenticated user in a specific course.
- **Request**: Course ID, user ID.
- **Response**: Enrollment confirmation.

---

## Installation and Setup

### Prerequisites
- .NET 6 or higher
- SQL Server
- Visual Studio or VS Code

### Steps to Run
1. Clone the repository:
   ```bash
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```bash
   cd InteractiveLearningSystem
   ```
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
4. Update the database connection string in `appsettings.json`.
5. Apply migrations:
   ```bash
   dotnet ef database update
   ```
6. Run the application:
   ```bash
   dotnet run
   ```

- **Portfolio**: [YourPortfolio.com](https://yourportfolio.com)
- **LinkedIn**: [Your LinkedIn](https://linkedin.com/in/yourlinkedin)
