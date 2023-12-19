namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentPriorityTypesController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public StudentPriorityTypesController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<StudentPriorityType>>> GetStudentPriorityTypesByStudentId(int studentId)
        {
            if (_context.StudentPriorityTypes == null)
            {
                return NotFound();
            }
            return await _context.StudentPriorityTypes.Where(spt => spt.StudentId == studentId).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Province>> PostStudentPriorityType(StudentPriorityType studentPriorityType)
        {
            if (_context.StudentPriorityTypes == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.StudentPriorityTypes'  is null.");
            }
            _context.StudentPriorityTypes.Add(studentPriorityType);
            await _context.SaveChangesAsync();

            return Ok(studentPriorityType);
        }

        [HttpDelete("{studentId}/{priorityTypeId}")]
        public async Task<IActionResult> DeleteStudentPriorityType(int studentId, int priorityTypeId)
        {
            if (_context.StudentPriorityTypes == null)
            {
                return NotFound();
            }

            var studentPriorityType = await _context.StudentPriorityTypes.FirstAsync(spt => spt.StudentId == studentId && spt.PriorityTypeId == priorityTypeId);
            if (studentPriorityType == null)
            {
                return NotFound();
            }

            _context.StudentPriorityTypes.Remove(studentPriorityType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
