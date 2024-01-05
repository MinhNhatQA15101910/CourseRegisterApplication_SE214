using CourseRegisterApplication.Shared;
using System.Net;

namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectTypesController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public SubjectTypesController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        private bool SubjectTypeExists(int id)
        {
            return (_context.SubjectTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectType>>> GetAllSubjectType()
        {
            try
            {
                if (_context.SubjectTypes == null)
                {
                    return new NotFoundResult();
                }

                var subjectTypes = await _context.SubjectTypes.ToListAsync();

                if (subjectTypes == null)
                {
                    return NotFound("No subject type found!");
                }

                return Ok(subjectTypes);

            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        
        [HttpGet("{subjectTypeId}")]
        public async Task<ActionResult<SubjectType>> GetAllSubjectTypeById(int subjectTypeId)
        {
            try
            {
                if (_context.SubjectTypes == null)
                {
                    return new NotFoundResult();
                }

                var subjectType = await _context.SubjectTypes.Where(st => st.Id == subjectTypeId).FirstOrDefaultAsync();

                if (subjectType == null)
                {
                    return NotFound("No subject type found!");
                }

                return Ok(subjectType);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubjectType>> CreateSubjectType(SubjectType subjectType)
        {
            if (_context.SubjectTypes == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.SubjectTypes'  is null.");
            }
            _context.SubjectTypes.Add(subjectType);
            await _context.SaveChangesAsync();

            return Ok(subjectType);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubjectType(int id, SubjectType subjectType)
        {
            if (id != subjectType.Id)
            {
                return BadRequest();
            }

            _context.Entry(subjectType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectTypeExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubjectType(int id)
        {
            if (_context.SubjectTypes == null)
            {
                return NotFound();
            }
            var subjectType = await _context.SubjectTypes.FindAsync(id);
            if (subjectType == null)
            {
                return NotFound();
            }

            _context.SubjectTypes.Remove(subjectType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
