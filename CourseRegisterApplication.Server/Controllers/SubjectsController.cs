namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public SubjectsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            if (_context.Subjects == null)
            {
                return NotFound();
            }
            return await _context.Subjects.ToListAsync();
        }
        [HttpGet("curriculum/{branchId}/{semester}")]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjectsByBranchAndSemester(int? branchId, int? semester)
        {
            if (_context.Curriculums == null)
            {
                return NotFound();
            }

            var query = _context.Curriculums.AsQueryable();

            if (branchId != null)
            {
                query = query.Where(c => c.BranchId == branchId);
            }

            if (semester != null)
            {
                query = query.Where(c => c.Semester == semester);
            }

            var subjects = await query
                .Select(c => c.Subject)
                .Distinct()
                .ToListAsync();

            return subjects;
        }

        [HttpGet("{subjectId}")]
        public async Task<ActionResult<Subject>> GetSubjectById(int subjectId)
        {
            if (_context.Subjects == null)
            {
                return NotFound();
            }

            var result = await _context.Subjects.FirstAsync(b => b.Id == subjectId);
            return result;
        }
    }
}
