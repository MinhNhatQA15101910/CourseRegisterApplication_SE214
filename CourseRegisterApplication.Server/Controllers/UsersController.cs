using Microsoft.AspNetCore.Mvc;

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
    }
}
