namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public DistrictsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }
    }
}
