using System.Collections.Generic;

namespace educoreLMS.Models.Entities
{
    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string StaffNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    }
}
