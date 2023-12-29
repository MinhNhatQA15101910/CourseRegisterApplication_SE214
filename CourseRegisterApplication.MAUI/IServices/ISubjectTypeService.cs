namespace CourseRegisterApplication.MAUI.IServices
{
    public interface ISubjectTypeService
    {
        Task<List<SubjectType>> GetAllSubjectType();
        Task<SubjectType> GetSubjectTypeById(int subjectTypeId);

    }
}
