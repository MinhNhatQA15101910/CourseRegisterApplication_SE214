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

        [HttpPost]
        public async Task<ActionResult<Province>> PostProvince(Province province)
        {
            if (_context.Provinces == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.Provinces'  is null.");
            }
            _context.Provinces.Add(province);
            await _context.SaveChangesAsync();

            return Ok(province);
        }

        [HttpPut("{provinceId}")]
        public async Task<IActionResult> PutProvince(int provinceId, Province province)
        {
            if (provinceId != province.Id)
            {
                return BadRequest();
            }

            _context.Entry(province).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinceExists(provinceId))
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

        private bool ProvinceExists(int provinceId)
        {
            return (_context.Provinces?.Any(p => p.Id == provinceId)).GetValueOrDefault();
        }
    }
}
