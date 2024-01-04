namespace CourseRegisterApplication.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SemestersController : ControllerBase
{
    private readonly CourseRegisterManagementDbContext _context;

    public SemestersController(CourseRegisterManagementDbContext context)
    {
        _context = context;
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
}
