namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISubjectTypeService
    {
        Task<List<SubjectType>> GetAllSubjectType();
        Task<SubjectType> GetSubjectTypeById(int subjectTypeId);
        Task<SubjectType> CreateSubjectType(SubjectType subjectType);
        Task<bool> UpdateSubjectType(int id, SubjectType subjectType);
        Task<bool> DeleteSubjectType(int id);
    }
}
