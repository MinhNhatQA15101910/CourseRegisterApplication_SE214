using CourseRegisterApplication.Shared;
using System.Security.Permissions;

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
            try
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
                return new NotFoundResult();
            }

            var subject = await _context.Subjects.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
            }

            _context.Entry(subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "Concurrency issue occurred");
                }
            }

            var result = await _context.Subjects.FirstAsync(b => b.Id == subjectId);
            return result;
        }
    }
}
