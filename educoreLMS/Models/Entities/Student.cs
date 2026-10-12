using System;

namespace educoreLMS.Models.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string HomeAddress { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public int? CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        public Guid Guid { get; set; } = Guid.NewGuid();
    }
}
