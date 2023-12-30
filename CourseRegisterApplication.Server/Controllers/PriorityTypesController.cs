namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityTypesController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

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
