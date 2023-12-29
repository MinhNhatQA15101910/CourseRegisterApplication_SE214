namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IPriorityTypeService
    {
        Task<IEnumerable<PriorityType>> GetPriorityTypesFromStudentIdAsync(int studentId);
    }
}
