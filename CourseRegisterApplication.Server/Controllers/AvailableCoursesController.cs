namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailableCoursesController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public AvailableCoursesController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AvailableCourse>>> GetAllAvailableCourse()
        {
            try
            {
                if (_context.AvailableCourses == null)
                {
                    return new NotFoundResult();
                }

                var availableCourses = await _context.AvailableCourses.ToListAsync();

                if (availableCourses == null)
                {
                    return NotFound("No available courses found!");
                }

                return Ok(availableCourses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{semesterId}/{subjectId}")]
        public async Task<IActionResult> DeleteAvaibleCourse(int semesterId, int subjectId)
        {
            var availableCourse = await _context.AvailableCourses
                .FirstAsync(ac => ac.SemesterId == semesterId && ac.SubjectId == subjectId);

            if (availableCourse == null)
            {
                return NotFound();
            }

            _context.AvailableCourses.Remove(availableCourse);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}