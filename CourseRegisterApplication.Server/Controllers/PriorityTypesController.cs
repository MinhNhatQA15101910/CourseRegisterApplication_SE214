namespace CourseRegisterApplication.Server.Controllers
{
    /// <summary>
    /// Api to get all things related to PriorityType
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityTypesController : ControllerBase
    {
        /// <summary>
        /// Database context of PriorityType api.
        /// </summary>
        private readonly CourseRegisterManagementDbContext _context;

        /// <summary>
        /// Constructor of PriorityType api.
        /// </summary>
        /// <param name="context">Database context injected from services.</param>
        public PriorityTypesController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get Priority Types from student Id
        /// </summary>
        /// <param name="studentId">ID of the student</param>
        /// <returns></returns>
        [HttpGet("studentId/{studentId}")]
        public async Task<ActionResult<IEnumerable<PriorityType>>> GetPriorityTypesByStudentId(int studentId)
        {
            var studentIdParameter = new SqlParameter("studentId", studentId);

            var priorityTypes = await _context.PriorityTypes
                    .FromSqlRaw(
                        "SELECT pt.* " +
                        "FROM dbo.StudentPriorityTypes spt, dbo.PriorityTypes pt " +
                        "WHERE spt.StudentId = @studentId and spt.PriorityTypeId = pt.Id",
                        studentIdParameter)
                    .ToListAsync();

            return Ok(priorityTypes);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriorityType>>> GetAllPriorityTypes()
        {
            try
            {
                if (_context.PriorityTypes == null)
                {
                    return new NotFoundResult();
                }

                var priorityTypes = await _context.PriorityTypes.ToListAsync();

                if (priorityTypes == null)
        {
                    return NotFound("Priority types found!");
                }

            return Ok(priorityTypes);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
