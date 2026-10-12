using educoreLMS.Data;
using educoreLMS.Dtos;
using educoreLMS.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace educoreLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public StudentController(ApplicationDbContext db) => _db = db;

        [HttpGet("{studentId}/tasks")]
        public async Task<IActionResult> GetTasks(int studentId, educoreLMS.Models.Entities.TaskStatus? status)
        {
            var query = _db.Tasks.Include(t => t.Module).Where(t => t.StudentId == studentId);

            if (status.HasValue) query = query.Where(t => t.Status == status.Value);
            return Ok(await query.ToListAsync());
        }

        [HttpPut("tasks/{taskId}/status")]
        public async Task<IActionResult> UpdateStatus(int taskId, [FromBody] UpdateTaskStatusDto dto)
        {
            var task = await _db.Tasks.FindAsync(taskId);
            if (task == null) return NotFound();
            task.Status = dto.Status;
            await _db.SaveChangesAsync();
            return Ok(task);
        }

        [HttpGet("by-guid/{guid}")]
        public async Task<IActionResult> GetStudentByGuid(string guid)
        {
            if (!Guid.TryParse(guid, out var studentGuid))
                return BadRequest(new { error = "Invalid GUID format." });

            var student = await _db.Students.FirstOrDefaultAsync(s => s.Guid == studentGuid);
            if (student == null)
                return NotFound(new { error = "Student not found." });

            return Ok(new { studentId = student.StudentId });
        }


        [HttpGet("{studentId}/detailed-tasks")]
        public async Task<IActionResult> GetDetailedTasks(int studentId)
        {
            var tasks = await _db.Tasks
                .Include(t => t.Module)
                    .ThenInclude(m => m.Course)
                .Where(t => t.StudentId == studentId)
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
        {
            var student = new Student
            {
                StudentNumber = dto.StudentNumber,
                Name = dto.Name,
                Surname = dto.Surname,
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                HomeAddress = dto.HomeAddress,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CourseId = dto.CourseId
            };
            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            return Ok(student);
        }

    }
}
