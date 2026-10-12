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
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public StudentsController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Students
             .Include(s => s.Course)
             .ThenInclude(c => c.Lecturer)
             .ToListAsync());


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _db.Students.Include(s => s.Course)
                .FirstOrDefaultAsync(s => s.StudentId == id);
            return student == null ? NotFound() : Ok(student);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? studentNumber, string? name, string? surname)
        {
            var query = _db.Students.AsQueryable();
            if (!string.IsNullOrEmpty(studentNumber))
                query = query.Where(s => s.StudentNumber.Contains(studentNumber));
            if (!string.IsNullOrEmpty(name))
                query = query.Where(s => s.Name.Contains(name));
            if (!string.IsNullOrEmpty(surname))
                query = query.Where(s => s.Surname.Contains(surname));
            return Ok(await query
             .Include(s => s.Course)
             .ThenInclude(c => c.Lecturer)
             .ToListAsync());

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateStudentDto dto)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.StudentNumber = dto.StudentNumber;
            student.Name = dto.Name;
            student.Surname = dto.Surname;
            student.Gender = dto.Gender;
            student.DateOfBirth = dto.DateOfBirth;
            student.HomeAddress = dto.HomeAddress;
            student.Email = dto.Email;
            student.PhoneNumber = dto.PhoneNumber;
            student.CourseId = dto.CourseId;

            await _db.SaveChangesAsync();
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();
            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
