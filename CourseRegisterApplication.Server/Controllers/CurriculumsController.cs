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

        [HttpGet("ByBranch/{branchId}")]
        public async Task<ActionResult<IEnumerable<Curriculum>>> GetCurriculumsByBranchId(int branchId)
        {
            try
            {
                if (_context.Curriculums == null)
                {
                    return new NotFoundResult();
                }

                var curriculums = await _context.Curriculums.Where(c => c.BranchId == branchId).ToListAsync();

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
    }
}
