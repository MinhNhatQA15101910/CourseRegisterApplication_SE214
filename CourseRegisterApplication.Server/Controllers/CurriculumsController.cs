namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public CurriculumsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("branch/{branchId}")]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetCurriculumsByBranchId(int branchId)
        {
            if (_context.Curriculums == null)
            {
                return NotFound();
            }

            var result = await _context.Curriculums.Where(c => c.BranchId == branchId).ToListAsync();
            return result;
        }
    }
}
