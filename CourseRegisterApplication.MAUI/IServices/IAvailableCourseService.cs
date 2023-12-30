namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IAvailableCourseService
    {
        Task<List<AvailableCourse>> GetAllAvailableCourse();
        Task<List<AvailableCourse>> GetAvailableCourseBySemesterId(int semesterId);
    }
}
