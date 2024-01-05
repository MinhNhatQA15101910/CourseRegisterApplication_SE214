namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IPriorityTypeService
    {
        Task<List<PriorityType>> GetAllPriorityType();
        Task<IEnumerable<PriorityType>> GetAllPriorityTypesAsync();
        Task<IEnumerable<PriorityType>> GetPriorityTypesFromStudentIdAsync(int studentId);
    }
}
