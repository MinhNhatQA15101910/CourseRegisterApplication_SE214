namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IStudentPriorityTypeService
    {
        Task<StudentPriorityType> AddStudentPriorityType(StudentPriorityType studentPriorityType);
        Task<bool> DeleteStudentPriorityType(int studentId, int priorityTypeId);
        Task<List<StudentPriorityType>> GetStudentPriorityTypesByStudentId(int studentId);
    }
}
