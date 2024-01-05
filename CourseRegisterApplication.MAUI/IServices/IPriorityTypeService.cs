namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IPriorityTypeService
    {
        Task<IEnumerable<PriorityType>> GetAllPriorityTypesAsync();
        Task<IEnumerable<PriorityType>> GetPriorityTypesFromStudentIdAsync(int studentId);
    }
}
