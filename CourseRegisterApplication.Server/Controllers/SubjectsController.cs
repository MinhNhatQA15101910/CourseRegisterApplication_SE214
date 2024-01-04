namespace CourseRegisterApplication.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly CourseRegisterManagementDbContext _context;

    public SubjectsController(CourseRegisterManagementDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Subject>>> GetAllSubjects()
    {
        if (_context.Subjects == null)
        {
            return NotFound();
        }
        
        List<Subject> subjectList = await _context.Subjects
            .FromSqlRaw(
                "SELECT s.* " +
                "FROM dbo.Subjects s")
            .ToListAsync();

        return Ok(subjectList);
    }

    [HttpGet("firstSemester")]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsForFirstSemester()
    {
        if (_context.Subjects == null)
        {
            return NotFound();
        }

        List<Subject> subjectList = await _context.Subjects
            .FromSqlRaw(
                "(SELECT s.* " +
                "FROM dbo.Subjects s, dbo.Curriculums c " +
                "WHERE s.Id = c.SubjectId " +
                "AND c.Semester % 2 = 1) " +
                "UNION " +
                "(SELECT s.* " +
                "FROM dbo.Subjects s " +
                "WHERE s.Id NOT IN " +
                "(SELECT c.SubjectId FROM dbo.Curriculums c))")
            .ToListAsync();

        return Ok(subjectList);
    }

    [HttpGet("secondSemester")]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsForSecondSemester()
    {
        if (_context.Subjects == null)
        {
            return NotFound();
        }

        List<Subject> subjectList = await _context.Subjects
            .FromSqlRaw(
                "(SELECT s.* " +
                "FROM dbo.Subjects s, dbo.Curriculums c " +
                "WHERE s.Id = c.SubjectId " +
                "AND c.Semester % 2 = 0) " +
                "UNION " +
                "(SELECT s.* " +
                "FROM dbo.Subjects s " +
                "WHERE s.Id NOT IN " +
                "(SELECT c.SubjectId FROM dbo.Curriculums c))")
            .ToListAsync();

        return Ok(subjectList);
    }

    [HttpGet("summerSemester")]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsForSummerSemester()
    {
        return await GetAllSubjects();
    }

    [HttpGet("semesterId/{semesterId}")]
    public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsBySemesterId(int semesterId)
    {
        if (_context.Subjects == null)
        {
            return NotFound();
        }

        SqlParameter parameter = new("semesterId", semesterId);
        List<Subject> subjectList = await _context.Subjects
            .FromSqlRaw(
                "SELECT s.* " +
                "FROM dbo.Subjects s, dbo.AvailableCourses ac " +
                "WHERE s.Id = ac.SubjectId " +
                "AND ac.SemesterId = @semesterId",
                parameter)
            .ToListAsync();

        return Ok(subjectList);
    }
}
