using System;
using educoreLMS.Models.Entities;

namespace educoreLMS.Dtos
{
    public class LoginDto { public string Email { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; }
    public class RegisterDto { public string Email { get; set; } = string.Empty; public string Password { get; set; } = string.Empty; public string Role { get; set; } = "Student"; public string? FirstName { get; set; } public string? LastName { get; set; } }

    public class CreateStudentDto
    {
        public string StudentNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string HomeAddress { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public int CourseId { get; set; }
        public string? Password { get; set; }
    }

    public class CreateLecturerDto
    {
        public string StaffNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }

    public class AssignLecturerDto
    {
        public int CourseId { get; set; }
        public int LecturerId { get; set; }
    }

    public class UpdateCourseDto
    {
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }


    public class CreateCourseDto { public string CourseName { get; set; } = string.Empty; public string? Description { get; set; } }
    public class CreateModuleDto { public string ModuleName { get; set; } = string.Empty; public string? Description { get; set; } }
    public class CreateTaskDto
    { 
        public string TaskName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int ModuleId { get; set; }
    }
    public class UpdateTaskStatusDto { public educoreLMS.Models.Entities.TaskStatus Status { get; set; } }
}
