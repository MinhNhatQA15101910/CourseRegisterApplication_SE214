using CourseRegisterApplication.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public StudentsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult<Student[]>> GetAllStudent()
        {
            if (_context.Students == null)
            {
                return new NotFoundResult();
            }

            var students = await _context.Students.ToListAsync();

            if (students == null)
            {
                return new NotFoundResult();
            }

            return Ok(students);
        }
    }
}
