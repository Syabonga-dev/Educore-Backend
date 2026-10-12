using educoreLMS.Data;
using educoreLMS.Dtos;
using educoreLMS.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace educoreLMS.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public CoursesController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _db.Courses.Include(c => c.Modules).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
        {
            var c = new Course { CourseName = dto.CourseName, Description = dto.Description };
            _db.Courses.Add(c);
            await _db.SaveChangesAsync();
            return Ok(c);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.CourseName = dto.CourseName;
            course.Description = dto.Description;

            await _db.SaveChangesAsync();
            return Ok(course);
        }


        [HttpPost("assign-lecturer")]
        public async Task<IActionResult> AssignLecturer([FromBody] AssignLecturerDto dto)
        {
            var course = await _db.Courses.FindAsync(dto.CourseId);
            var lecturer = await _db.Lecturers.FindAsync(dto.LecturerId);
            if (course == null || lecturer == null) return NotFound("Course or Lecturer not found.");

            course.LecturerId = lecturer.LecturerId;
            await _db.SaveChangesAsync();
            return Ok(course);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _db.Courses
                .Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
                return NotFound("Course not found.");

            
            if (course.Modules != null && course.Modules.Any())
            {
                _db.Modules.RemoveRange(course.Modules);
            }

            _db.Courses.Remove(course);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Course deleted successfully." });
        }

    }
}