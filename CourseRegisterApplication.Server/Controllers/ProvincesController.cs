namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvincesController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public ProvincesController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Province>>> GetAllProvinces()
        {
            if (_context.Provinces == null)
            {
                return NotFound();
            }
            return await _context.Provinces.ToListAsync();
        }

        [HttpGet("{provinceId}")]
        public async Task<ActionResult<Province>> GetProvinceById(int provinceId)
        {
            if (_context.Provinces == null)
            {
                return NotFound();
            }

            var province = await _context.Provinces.FindAsync(provinceId);
            if (province == null)
            {
                return NotFound();
            }

            return province;
        }

        [HttpDelete("{provinceId}")]
        public async Task<IActionResult> DeleteProvince(int provinceId)
        {
            if (_context.Provinces == null)
            {
                return NotFound();
            }
            var province = await _context.Provinces.FindAsync(provinceId);
            if (province == null)
            {
                return NotFound();
            }

            _context.Provinces.Remove(province);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
