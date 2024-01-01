namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IAvailableCourseService
    {
        Task<List<AvailableCourse>> GetAllAvailableCourse();
        Task<List<AvailableCourse>> GetAvailableCourseBySemesterId(int semesterId);
        Task<List<AvailableCourse>> GetAvailableCourseBySubjectId(int subjectId);
        Task<bool> DeleteAvailaleCourse(int semesterId, int subjectId);
    }
}
