﻿namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public StudentsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public async Task<ActionResult<Student[]>> GetAllStudents()
        {
            if (_context.Students == null)
            {
                return new NotFoundResult();
            }

            var students = await _context.Students.ToListAsync();

            if (students == null)
            {
                return new NotFoundResult();
            }

            return Ok(students);
        }

        [HttpGet("{studentSpecificId}")]
        public async Task<ActionResult<Student>> GetStudentBySpecificID(string studentSpecificId)
        {
            try
            {
                if (_context.Students == null)
                {
                    return new NotFoundResult();
                }

                var student = await _context.Students.Where(s => s.StudentSpecificId == studentSpecificId).FirstOrDefaultAsync();

                if (student == null)
                {
                    return NotFound("No student have that ID, please search again!");
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("branch/{branchId}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByBranchId(int branchId)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }

            var result = await _context.Students.Where(s => s.BranchId == branchId).ToListAsync();
            return result;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateStudent([FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Invalid student data");
                }

                // Assuming Student class has an autogenerated Id property
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // Assuming you have a route named "GetStudent" to retrieve the created student by ID
                return CreatedAtAction("GetStudentBySpecificID", new { studentSpecificId = student.StudentSpecificId }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}