using System.Collections.Generic;

namespace educoreLMS.Models.Entities
{
    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
