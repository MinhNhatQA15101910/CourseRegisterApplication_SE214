using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public SemestersController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Semester>>> GetAllSemester()
        {
            try
            {
                if (_context.Semesters == null)
                {
                    return new NotFoundResult();
                }

                var semesters = await _context.Semesters.ToListAsync();

                if (semesters == null)
                {
                    return NotFound("No semester found!");
                }

                return Ok(semesters);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("GetSemesterByNameAndYear")]
        public async Task<ActionResult<Semester>> GetSemesterByNameAndYear([FromQuery] SemesterName? name, [FromQuery] int year)
        {
            try
            {
                if (_context.Semesters == null)
                {
                    return new NotFoundResult();
                }

                var semesters = await _context.Semesters.ToListAsync();
                var querySemester = new Semester();

                if (name.HasValue && !string.IsNullOrEmpty(year.ToString()))
                {
                    querySemester = semesters.Where(s => s.SemesterName == name).FirstOrDefault();
                }

                if (querySemester == null)
                {
                    return NotFound("No semester of that name and year found!");
                }

                return Ok(querySemester);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
