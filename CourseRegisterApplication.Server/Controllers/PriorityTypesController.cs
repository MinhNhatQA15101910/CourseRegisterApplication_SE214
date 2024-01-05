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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriorityType>>> GetAllPriorityType()
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
