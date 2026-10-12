using System.Reflection;
using System.Collections.Generic;

namespace educoreLMS.Models.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? LecturerId { get; set; }
        public virtual Lecturer? Lecturer { get; set; }

        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}
