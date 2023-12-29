namespace CourseRegisterApplication.Server.Controllers
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

        [HttpGet]
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

        [HttpGet("specificId/{studentSpecificId}")]
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

        /// <summary>
        /// Return full information of a student.
        /// </summary>
        /// <param name="studentSpecificId">Specific Id of student.</param>
        /// <returns></returns>
        [HttpGet("full/specificId/{studentSpecificId}")]
        public async Task<ActionResult<Student>> GetFullInformationOfStudentBySpecificID(string studentSpecificId)
        {
            // Get student
            var parameter = new SqlParameter("studentSpecificId", studentSpecificId);
            Student result = await _context.Students
                    .FromSqlRaw(
                        "SELECT * FROM dbo.Students s " +
                        "WHERE s.StudentSpecificId = @studentSpecificId",
                        parameter)
                    .FirstAsync();
            if (result == null)
            {
                return NotFound();
            }

            // Get branch
            parameter = new SqlParameter("branchId", result.BranchId);
            result.Branch = await _context.Branches
                    .FromSqlRaw(
                        "SELECT * FROM dbo.Branches b " +
                        "WHERE b.Id = @branchId",
                        parameter)
                    .FirstAsync();
            if (result.Branch != null)
            {
                // Get department
                parameter = new SqlParameter("departmentId", result.Branch.DepartmentId);
                result.Branch.Department = await _context.Departments
                        .FromSqlRaw(
                            "SELECT * FROM dbo.Departments d " +
                            "WHERE d.Id = @departmentId",
                            parameter)
                        .FirstAsync();
                if (result.Branch.Department == null)
                {
                    return NotFound();
                }
            }

            // Get district
            parameter = new SqlParameter("districtId", result.DistrictId);
            result.District = await _context.Districts
                    .FromSqlRaw(
                        "SELECT * FROM dbo.Districts d " +
                        "WHERE d.Id = @districtId",
                        parameter)
                    .FirstAsync();
            if (result.District != null)
            {
                // Get province
                parameter = new SqlParameter("provinceId", result.District.ProvinceId);
                result.District.Province = await _context.Provinces
                        .FromSqlRaw(
                            "SELECT * FROM dbo.Provinces p " +
                            "WHERE p.Id = @provinceId",
                            parameter)
                        .FirstAsync();
                if (result.District.Province == null)
                {
                    return NotFound();
                }
            }

            // Get priority type list
            string json = JsonConvert.SerializeObject(result, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Ok(json);
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

        [HttpGet("district/{districtId}")]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByDistrictId(int districtId)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }

            var result = await _context.Students.Where(s => s.DistrictId == districtId).ToListAsync();
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

        [HttpPut("{studentId}")]
        public async Task<IActionResult> PutStudent(int studentId, Student student)
        {
            if (studentId != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(studentId))
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

        private bool StudentExists(int studentId)
        {
            return (_context.Students?.Any(s => s.Id == studentId)).GetValueOrDefault();
        }
    }
}