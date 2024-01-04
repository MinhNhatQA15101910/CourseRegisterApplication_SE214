namespace CourseRegisterApplication.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TuitionFeeReceiptsController : ControllerBase
    {
        private readonly CourseRegisterManagementDbContext _context;

        public TuitionFeeReceiptsController(CourseRegisterManagementDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TuitionFeeReceipt>>> GetAllTuitionFeeReceipt()
        {
            try
            {
                if (_context.TuitionFeeReceipts == null)
                {
                    return new NotFoundResult();
                }

                var tuitionFeeReceipts = await _context.TuitionFeeReceipts.ToListAsync();

                if (tuitionFeeReceipts == null)
                {
                    return NotFound("No tuition fee receipts found!");
                }

                return Ok(tuitionFeeReceipts);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
