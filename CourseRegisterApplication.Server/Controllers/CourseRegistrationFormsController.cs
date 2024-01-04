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

        private bool CourseRegiatrationFormExists(int id)
        {
            return (_context.CourseRegistrationForms?.Any(e => e.Id == id)).GetValueOrDefault();
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

                var courseRegistrationForms = await _context.CourseRegistrationForms.ToListAsync();

                if (courseRegistrationForms == null)
                {
                    return NotFound("No course registration form found!");
                }

                return Ok(courseRegistrationForms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseRegistrationForm>> GetCourseRegistrationFormById(int id)
        {
            try
            {
                if (_context.CourseRegistrationForms == null)
                {
                    return new NotFoundResult();
                }

                var courseRegistrationForm = await _context.CourseRegistrationForms.Where(crf => crf.Id == id).FirstOrDefaultAsync();

                if (courseRegistrationForm == null)
                {
                    return NotFound("No course registration form of that id found!");
                }

                return Ok(courseRegistrationForm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseRegistrationForm>> GetCourseRegistrationFormById(int id)
        {
            try
            {
                if (_context.CourseRegistrationForms == null)
                {
                    return new NotFoundResult();
                }

                var courseRegistrationForm = await _context.CourseRegistrationForms.Where(crf => crf.Id == id).FirstOrDefaultAsync();

                if (courseRegistrationForm == null)
                {
                    return NotFound("No course registration form of that id found!");
                }

                return Ok(courseRegistrationForm);
            }
            catch (Exception ex)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourseRegistrationForm(int id, CourseRegistrationForm courseRegistrationForm)
        {
            if (id != courseRegistrationForm.Id)
            {
                return BadRequest();
            }

            _context.Entry(courseRegistrationForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseRegiatrationFormExists(id))
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
    }
}