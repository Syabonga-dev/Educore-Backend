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
    public class LecturersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public LecturersController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Lecturers.Include(l => l.Courses).ToListAsync());

        [HttpGet("search")]
        public async Task<IActionResult> Search(string? staffNumber, string? name, string? surname)
        {
            var query = _db.Lecturers.AsQueryable();
            if (!string.IsNullOrEmpty(staffNumber))
                query = query.Where(l => l.StaffNumber.Contains(staffNumber));
            if (!string.IsNullOrEmpty(name))
                query = query.Where(l => l.Name.Contains(name));
            if (!string.IsNullOrEmpty(surname))
                query = query.Where(l => l.Surname.Contains(surname));
            return Ok(await query.ToListAsync());
        }

        

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateLecturerDto dto)
        {
            var lecturer = await _db.Lecturers.FindAsync(id);
            if (lecturer == null) return NotFound();

            lecturer.StaffNumber = dto.StaffNumber;
            lecturer.Name = dto.Name;
            lecturer.Surname = dto.Surname;
            lecturer.Email = dto.Email;
            lecturer.PhoneNumber = dto.PhoneNumber;

            await _db.SaveChangesAsync();
            return Ok(lecturer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lecturer = await _db.Lecturers.FindAsync(id);
            if (lecturer == null) return NotFound();
            _db.Lecturers.Remove(lecturer);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
