using educoreLMS.Data;
using educoreLMS.Dtos;
using educoreLMS.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace educoreLMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Lecturer")]
    public class LecturerController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public LecturerController(ApplicationDbContext db) => _db = db;

        
        private async Task<Lecturer?> GetLecturerFromClaims()
        {
            var emailClaim = User.Claims
                                 .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            if (string.IsNullOrEmpty(emailClaim))
                return null;

            return await _db.Lecturers.FirstOrDefaultAsync(l => l.Email == emailClaim);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> MyCourses()
        {
            var lecturer = await GetLecturerFromClaims();
            if (lecturer == null)
                return Unauthorized();

            var courses = await _db.Courses
                                   .Include(c => c.Modules)
                                   .Where(c => c.LecturerId == lecturer.LecturerId)
                                   .ToListAsync();
            return Ok(courses);
        }

        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            var lecturer = await GetLecturerFromClaims();
            if (lecturer == null)
                return Unauthorized();

            var module = await _db.Modules
                                  .Include(m => m.Course)
                                  .FirstOrDefaultAsync(m => m.ModuleId == dto.ModuleId && m.Course.LecturerId == lecturer.LecturerId);

            if (module == null)
                return BadRequest("Invalid module or not assigned to you.");

            var students = await _db.Students
                                    .Where(s => s.CourseId == module.CourseId)
                                    .ToListAsync();

            if (students.Count == 0)
                return BadRequest("No students enrolled in this module's course.");

            var tasks = students.Select(s => new TaskItem
            {
                TaskName = dto.TaskName,
                DueDate = dto.DueDate,
                ModuleId = dto.ModuleId,
                StudentId = s.StudentId
            }).ToList();

            _db.Tasks.AddRange(tasks);
            await _db.SaveChangesAsync();

            return Ok(new { Message = $"Task '{dto.TaskName}' assigned to {students.Count} students." });
        }

        [HttpGet("modules/{moduleId}/tasks")]
        public async Task<IActionResult> GetTasks(int moduleId)
        {
            var lecturer = await GetLecturerFromClaims();
            if (lecturer == null)
                return Unauthorized();

            var module = await _db.Modules
                                  .Include(m => m.Course)
                                  .FirstOrDefaultAsync(m => m.ModuleId == moduleId && m.Course.LecturerId == lecturer.LecturerId);

            if (module == null)
                return BadRequest("Invalid module.");

            var tasks = await _db.Tasks
                                 .Where(t => t.ModuleId == moduleId)
                                 .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLecturerDto dto)
        {
            var lecturer = new Lecturer
            {
                StaffNumber = dto.StaffNumber,
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };
            _db.Lecturers.Add(lecturer);
            await _db.SaveChangesAsync();
            return Ok(lecturer);
        }
    }
}
