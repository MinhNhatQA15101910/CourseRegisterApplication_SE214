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

        [HttpDelete("{districtId}")]
        public async Task<IActionResult> DeleteDistrict(int districtId)
        {
            if (_context.Districts == null)
            {
                return NotFound();
            }
            var district = await _context.Districts.FindAsync(districtId);
            if (district == null)
            {
                return NotFound();
            }

            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<District>> PostDistrict(District district)
        {
            if (_context.Districts == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.Districts'  is null.");
            }
            _context.Districts.Add(district);
            await _context.SaveChangesAsync();

            return Ok(district);
        }

        [HttpPut("{districtId}")]
        public async Task<IActionResult> PutDistrict(int districtId, District district)
        {
            if (districtId != district.Id)
            {
                return BadRequest();
            }

            _context.Entry(district).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictExists(districtId))
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

        private bool DistrictExists(int districtId)
        {
            return (_context.Districts?.Any(p => p.Id == districtId)).GetValueOrDefault();
        }
    }
}
