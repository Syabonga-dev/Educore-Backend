using educoreLMS.Models.Entities;
using System;

namespace educoreLMS.Models.Entities
{
    public enum TaskStatus
    {
        NotStarted = 0,
        InProgress = 1,
        Complete = 2
    }

    public class TaskItem
    {
        public int TaskItemId { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;

        public int ModuleId { get; set; }
        public virtual Module Module { get; set; } = null!;

        public int? StudentId { get; set; }
        public virtual Student? Student { get; set; }
    }
}
