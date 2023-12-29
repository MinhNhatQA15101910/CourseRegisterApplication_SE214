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
        public async Task<ActionResult<IEnumerable<SubjectType>>> GetAllSubjectTypeById(int subjectTypeId)
        {
            try
            {
                if (_context.SubjectTypes == null)
                {
                    return new NotFoundResult();
                }

                var subjectTypes = await _context.SubjectTypes.Where(st => st.Id == subjectTypeId).FirstOrDefaultAsync();

                if (subjectTypes == null)
                {
                    return NotFound("No subject type found!");
                }

                return Ok(subjectTypes);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
