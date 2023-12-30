using CourseRegisterApplication.Shared;
using System.Runtime.InteropServices;

namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseRegistrationFormsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public CourseRegistrationFormsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseRegistrationForm>>> GetAllCourseRegistrationForm()
        {
            try
            {
                if (_context.CourseRegistrationForms == null)
                {
                    return new NotFoundResult();
                }

                var courseRegistrationForm = await _context.CourseRegistrationForms.ToListAsync();

                if (courseRegistrationForm == null)
                {
                    return NotFound("No course registration form found!");
                }

                return Ok(courseRegistrationForm);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CourseRegistrationForm>> CreateCourseRegistrationForm(CourseRegistrationForm courseRegistrationForm)
        {
            if (_context.CourseRegistrationForms == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.CourseRegistrationForm'  is null.");
            }
            _context.CourseRegistrationForms.Add(courseRegistrationForm);
            await _context.SaveChangesAsync();

            return Ok(courseRegistrationForm);
        }
    }
}
