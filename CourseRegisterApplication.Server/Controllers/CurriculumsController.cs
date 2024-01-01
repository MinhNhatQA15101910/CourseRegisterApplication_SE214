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

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetAllCurriculums()
        {
            try
            {
                if (_context.Curriculums == null)
                {
                    return new NotFoundResult();
                }

                var curriculums = await _context.Curriculums.ToListAsync();

                if (curriculums == null)
                {
                    return NotFound("No curriculum found, please search again!");
                }

                return Ok(curriculums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("ByBranch/{branchId}")]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetCurriculumsByBranchId(int branchId)
        {
            if (_context.Curriculums == null)
            {
                return NotFound();
            }

            var result = await _context.Curriculums.Where(b => b.BranchId == branchId).ToListAsync();
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Curriculum>> PostCurriculum(Curriculum curriculum)
        {
            if (_context.Curriculums == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.Curriculums'  is null.");
            }
            _context.Curriculums.Add(curriculum);
            await _context.SaveChangesAsync();

            return Ok(curriculum);
        }

        // DELETE: api/Curriculums/
        [HttpDelete("{branchId}/{subjectId}")]
        public async Task<IActionResult> DeleteCurriculum(int branchId, int subjectId)
        {
            var curriculum = await _context.Curriculums
                .FindAsync(branchId, subjectId);

            if (curriculum == null)
            {
                return NotFound(); // Curriculum not found
            }

            _context.Curriculums.Remove(curriculum);
            await _context.SaveChangesAsync();

            return NoContent(); // Successfully deleted
        }

        [HttpGet("BySubject/{subjectId}")]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetCurriculumsBySubjectId(int subjectId)
        {
            try
            {
                if (_context.Curriculums == null)
                {
                    return new NotFoundResult();
                }

                var curriculums = await _context.Curriculums.Where(c => c.SubjectId == subjectId).ToListAsync();

                if (curriculums == null)
                {
                    return NotFound("No curriculum found, please search again!");
                }

                return Ok(curriculums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{branchId}/{subjectId}")]
        public async Task<IActionResult> DeleteCurriculum(int branchId, int subjectId)
        {
            var curriculum = await _context.Curriculums.FirstAsync(c => c.BranchId == branchId && c.SubjectId == subjectId);

            if (curriculum == null)
            {
                return NotFound("No curriculum of those Ids found!");
            }

            _context.Curriculums.Remove(curriculum);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}