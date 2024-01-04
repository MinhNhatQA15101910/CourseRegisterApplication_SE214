namespace CourseRegisterApplication.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvailableCoursesController : ControllerBase
{
    private readonly CourseRegisterManagementDbContext _context;

    public AvailableCoursesController(CourseRegisterManagementDbContext context)
    {
        _context = context;
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
