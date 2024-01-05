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
            }
            catch (Exception ex)
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
                    querySemester = semesters.Find(s => s.SemesterName == name);
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

        [HttpGet("current")]
        public async Task<ActionResult<Semester>> GetCurrentSemester()
        {
            if (_context.Semesters == null)
            {
                return NotFound();
            }
            var semester = await _context.Semesters
                .FromSqlRaw(
                    "SELECT * " +
                    "FROM dbo.Semesters s " +
                    "WHERE s.Id = " +
                        "(SELECT MAX(Id) " +
                        "FROM dbo.Semesters)"
                )
                .FirstAsync();

            return Ok(semester);
        }

        [HttpPut("{semesterId}")]
        public async Task<IActionResult> PutSemester(int semesterId, Semester semester)
        {
            if (semesterId != semester.Id)
            {
                return BadRequest();
            }

            _context.Entry(semester).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SemesterExists(semesterId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Semester>> PostSemester(Semester semester)
        {
            if (_context.Branches == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.Semesters'  is null.");
            }
            _context.Semesters.Add(semester);
            await _context.SaveChangesAsync();

            return Ok(semester);
        }

        private bool SemesterExists(int semesterId)
        {
            return (_context.Semesters?.Any(p => p.Id == semesterId)).GetValueOrDefault();
        }
    }
}