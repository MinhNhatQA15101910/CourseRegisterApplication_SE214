namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ITuitionFeeReceiptService
    {
        Task<List<TuitionFeeReceipt>> GetAllTuitionFeeReceipt();
        Task<List<TuitionFeeReceipt>> GetTuitionFeeReceiptsByCourseRegistrationFormId(int courseRegistrationFormId);
    }
}
