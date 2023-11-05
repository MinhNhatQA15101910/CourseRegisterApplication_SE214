using Microsoft.AspNetCore.Mvc;

namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public RolesController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            if (_context.Roles == null)
            {
                return new NotFoundResult();
            }
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(role);
        }
    }
}
