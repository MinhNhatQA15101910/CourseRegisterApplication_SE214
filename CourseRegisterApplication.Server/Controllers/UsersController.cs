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

        // Get user account by id
        // Use for CreatedAtAction() in POST method
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                if (_context.Users == null)
                {
                    return new NotFoundResult();
                }
                var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

                if (user == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /* 
         * Get all users if there is no given params
         * roleId: 0 - all, 1 - admin, 2 - accountant, 3 - student.
         * filter: based on username and email
         * NOTE: roleId = 0 is init for dev mode, Database contains 1, 2, 3.
         */
        [HttpGet()]
        public async Task<ActionResult<User[]>> GetUsersOptionalByRoleFilter([FromQuery] int roleId = 0, [FromQuery] string filter = "")
        {
            try
            {
                var query = _context.Users.Where(u => true);

                //// RoleId param check
                //if (roleId == 1 || roleId == 2 || roleId == 3)
                //{
                //    query = _context.Users.Where(u => u.RoleId == roleId);
                //}

                // Filter param check
                if (!string.IsNullOrEmpty(filter))
                {
                    query = query.Where(u => (u.Username != null && u.Username.Contains(filter)) || (u.Email != null && u.Email.Contains(filter)));
                }

                var accounts = await query.ToListAsync();

                if (accounts.Count == 0)
                {
                    return NotFound("No accounts found!");
                }

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // Create a new account 
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("Invalid user data.");
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetUserById", new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // Delete a user
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound(); // If the student with the given ID is not found, return 404 Not Found
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent(); // 204 No Content indicates successful deletion
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
