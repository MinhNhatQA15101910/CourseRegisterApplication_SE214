using CourseRegisterApplication.Shared;

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

        private bool TuitionFeeReceiptExists(int id)
        {
            return (_context.TuitionFeeReceipts?.Any(e => e.Id == id)).GetValueOrDefault();
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
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TuitionFeeReceipt>> CreateTuitionFeeReceipt(TuitionFeeReceipt tuitionFeeReceipt)
        {
            if (_context.TuitionFeeReceipts == null)
            {
                return Problem("Entity set 'CourseRegisterManagementDbContext.TuitionFeeReceipts'  is null.");
            }

            tuitionFeeReceipt.TuitionFeeReceiptSpecificId = $"TFR{_context.TuitionFeeReceipts.Count() + 1}";
            _context.TuitionFeeReceipts.Add(tuitionFeeReceipt);
            await _context.SaveChangesAsync();

            return Ok(tuitionFeeReceipt);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubjectType(int id, TuitionFeeReceipt tuitionFeeReceipt)
        {
            if (id != tuitionFeeReceipt.Id)
            {
                return BadRequest();
            }

            _context.Entry(tuitionFeeReceipt).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TuitionFeeReceiptExists(id))
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
    }
}