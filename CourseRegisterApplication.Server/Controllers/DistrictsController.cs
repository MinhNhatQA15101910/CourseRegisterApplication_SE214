namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public DistrictsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("province/{provinceId}")]
        public async Task<ActionResult<IEnumerable<District>>> GetDistrictsByProvinceId(int provinceId)
        {
            if (_context.Districts == null)
            {
                return NotFound();
            }

            var result = await _context.Districts.Where(d => d.ProvinceId == provinceId).ToListAsync();
            return result;
        }
    }
}
