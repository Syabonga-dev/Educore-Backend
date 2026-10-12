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
    public class ModulesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ModulesController(ApplicationDbContext db) => _db = db;

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetModulesByCourse(int courseId) =>
            Ok(await _db.Modules.Where(m => m.CourseId == courseId).Include(m => m.Course).ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModuleDto dto, int courseId)
        {
            var module = new Module { ModuleName = dto.ModuleName, Description = dto.Description, CourseId = courseId };
            _db.Modules.Add(module);
            await _db.SaveChangesAsync();
            return Ok(module);
        }

    }
}
