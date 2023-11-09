using CourseRegisterApplication.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8600 

namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public UsersController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<User>> LoginUser(string username, string password)
        {
            if (_context.Users == null)
            {
                return new NotFoundResult();
            }
            var user = await _context.Users.Where(u => u.Username == username && u.Password == password).FirstOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(user);
        }

        // Displaying accounts based on roleId 1 - admin, 2 - accountant, 3 - student
        [HttpGet()]
        public async Task<ActionResult<User[]>> GetAccountsByRole([FromQuery] int roleId)
        {
            try
            {
                // Check if the 'roleId' query parameter is provided.
                if (string.IsNullOrEmpty(roleId.ToString()))
                {
                    return BadRequest("Role ID is required.");
                }

                var accounts = await _context.Users.Where(u => u.RoleId == roleId).ToListAsync();

                if (accounts == null)
                {
                    return NotFound("No accounts found!");
                }

                // Create custom responses with adjusted properties
                IEnumerable<object> response = null;
                switch (roleId)
                {
                    case 1:
                        response = accounts.Select(s => new
                        {
                            id = s.Id,
                            username = s.Username,
                            email = s.Email,
                            accountType = "Admin"
                        });
                        break;
                    case 2:
                        response = accounts.Select(s => new
                        {
                            id = s.Id,
                            username = s.Username,
                            email = s.Email,
                            accountType = "Accountant"
                        });
                        break;
                    case 3:
                        response = accounts.Select(s => new
                        {
                            id = s.Id,
                            studendSpecificId = s.Username,
                            email = s.Email,
                        });
                        break;
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}
