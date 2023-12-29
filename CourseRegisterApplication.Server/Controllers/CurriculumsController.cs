﻿namespace CourseRegisterApplication.Server.Controllers
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

        [HttpGet("{branchId}")]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetCurriculumsByBranchId(int branchId)

        // GET: api/Curriculums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetCurriculums()
        {
            try
            {
                if (_context.Curriculums == null)
                {
                    return new NotFoundResult();
                }

                var curriculums = await _context.Curriculums.Where(c => c.BranchId == branchId).FirstOrDefaultAsync();

                if (curriculums == null)
                {
                    return NotFound("No curriculum found, please search again!");
                }
            if (_context.Curriculums == null)
            {
                return NotFound();
            }
            return await _context.Curriculums.ToListAsync();
        }

                return Ok(curriculums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        // POST: api/Curriculum
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

    }
}
