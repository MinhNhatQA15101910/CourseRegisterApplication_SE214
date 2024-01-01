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

        private bool SubjectExists(int id)
        {
            return (_context.Subjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetAllSubject()
        {
            try
            {
                if (_context.Subjects == null)
                {
                    return new NotFoundResult();
                }

                var subjects = await _context.Subjects.ToListAsync();

                if (subjects == null)
                {
                    return NotFound("No subjects found!");
                }

                return Ok(subjects);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubjectById(int id)
        {
            try
            {
                if (_context.Subjects == null)
                {
                    return new NotFoundResult();
                }

                var subject = await _context.Subjects.Where(s => s.Id == id).FirstOrDefaultAsync();

                if (subject == null)
                {
                    return NotFound("No subject of that Id found!");
                }

                return Ok(subject);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
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
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
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

    }
}
