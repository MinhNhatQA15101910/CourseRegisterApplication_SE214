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

        [HttpPost]
        public async Task<ActionResult<AvailableCourse>> PostAvailableCourse(AvailableCourse availableCourse)
        {
            if (_context.AvailableCourses == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.AvailableCourses'  is null.");
            }
            _context.AvailableCourses.Add(availableCourse);
            await _context.SaveChangesAsync();

            return Ok(availableCourse);
        }

        [HttpDelete("semesterId/{semesterId}")]
        public async Task<IActionResult> DeleteAvailableCoursesBySemesterId(int semesterId)
        {
            if (_context.AvailableCourses == null)
            {
                return NotFound();
            }
            var availableCourseList = _context.AvailableCourses.Where(ac => ac.SemesterId == semesterId);
            if (!availableCourseList.Any())
            {
                return NotFound();
            }

            _context.AvailableCourses.RemoveRange(availableCourseList);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}