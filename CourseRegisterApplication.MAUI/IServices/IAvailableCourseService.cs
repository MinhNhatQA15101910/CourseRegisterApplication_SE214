namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IAvailableCourseService
    {
        Task<AvailableCourse> AddAvailableCourseAsync(AvailableCourse availableCourse);
        Task<bool> DeleteAvailableCoursesBySemesterIdAsync(int semesterId);
        Task<List<AvailableCourse>> GetAllAvailableCourse();
        Task<List<AvailableCourse>> GetAvailableCourseBySemesterId(int semesterId);
        Task<List<AvailableCourse>> GetAvailableCourseBySubjectId(int subjectId);
        Task<bool> DeleteAvailaleCourse(int semesterId, int subjectId);
    }
}
