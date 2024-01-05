namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseRegistrationDetailsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public CourseRegistrationDetailsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseRegistrationDetail>>> GetAllCourseRegistrationDetail()
        {
            try
            {
                if (_context.CourseRegistrationDetails == null)
                {
                    return new NotFoundResult();
                }

                var courseRegistrationDetails = await _context.CourseRegistrationDetails.ToListAsync();

                if (courseRegistrationDetails == null)
                {
                    return NotFound("No details found!");
                }

                return Ok(courseRegistrationDetails);
            }catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CourseRegistrationDetail>>> GetCRDByCRFId(int crfId)
        {
            try
            {
                if (_context.CourseRegistrationDetails == null)
                {
                    return new NotFoundResult();
                }

                var courseRegistrationDetails = await _context.CourseRegistrationDetails.Where(crd => crd.CourseRegistrationFormId == crfId).ToListAsync();

                if (courseRegistrationDetails == null)
                {
                    return NotFound("No course registration detail of that Id found!");
                }

                return Ok(courseRegistrationDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CourseRegistrationDetail>> CreateCourseRegistrationDetail(CourseRegistrationDetail courseRegistrationDetail)
        {
            if (_context.CourseRegistrationDetails == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.CourseRegistrationDetail'  is null.");
            }
            _context.CourseRegistrationDetails.Add(courseRegistrationDetail);
            await _context.SaveChangesAsync();

            return Ok(courseRegistrationDetail);
        }

        [HttpDelete("{courseRegistrationFormId}/{subjectId}")]
        public async Task<IActionResult> DeleteCourseRegistrationDetail(int courseRegistrationFormId, int subjectId)
        {
            var courseRegistrationDetail = await _context.CourseRegistrationDetails
                .FirstAsync(c => c.CourseRegistrationFormId == courseRegistrationFormId && c.SubjectId == subjectId);

            if (courseRegistrationDetail == null)
            {
                return NotFound();
            }

            _context.CourseRegistrationDetails.Remove(courseRegistrationDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
