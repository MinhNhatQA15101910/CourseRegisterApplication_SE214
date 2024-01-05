namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public BranchesController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Branch>>> GetAllBranches()
        {
            if (_context.Branches == null)
            {
                return NotFound();
            }
            return await _context.Branches.ToListAsync();
        }

        [HttpGet("{branchId}")]
        public async Task<ActionResult<Branch>> GetBranchById(int branchId)
        {
            if (_context.Branches == null)
            {
                return NotFound();
            }

            var result = await _context.Branches.FirstAsync(b => b.Id == branchId);
            return result;
        }

        [HttpGet("department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranchesByDepartmentId(int departmentId)
        {
            if (_context.Branches == null)
            {
                return NotFound();
            }

            var result = await _context.Branches.Where(b => b.DepartmentId == departmentId).ToListAsync();
            return result;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranch(int id, Branch branch)
        {
            if (id != branch.Id)
            {
                return BadRequest();
            }

            _context.Entry(branch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchExists(id))
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

        [HttpPost]
        public async Task<ActionResult<Branch>> PostBranch(Branch branch)
        {
            if (_context.Branches == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.Branches'  is null.");
            }
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();

            return Ok(branch);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            if (_context.Branches == null)
            {
                return NotFound();
            }
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)    
            {
                return NotFound();
            }

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BranchExists(int id)
        {
            return (_context.Branches?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
