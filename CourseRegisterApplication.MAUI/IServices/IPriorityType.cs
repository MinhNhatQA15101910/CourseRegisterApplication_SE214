namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IPriorityTypeService
    {
        Task<List<PriorityType>> GetAllPriorityType();
    }
}
