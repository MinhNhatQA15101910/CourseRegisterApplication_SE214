namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ITuitionFeeReceiptService
    {
        Task<List<TuitionFeeReceipt>> GetAllTuitionFeeReceipt();
        Task<List<TuitionFeeReceipt>> GetTuitionFeeReceiptsByCourseRegistrationFormId(int courseRegistrationFormId);
        Task<TuitionFeeReceipt> CreateTuitionFeeReceipt(TuitionFeeReceipt tuitionFeeReceipt);
        Task<bool> UpdateTuitionFeeReceipt(int id, TuitionFeeReceipt tuitionFeeReceipt);
    }
}
