namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IAvailableCourseService
    {
        Task<AvailableCourse> AddAvailableCourseAsync(AvailableCourse availableCourse);
        Task<bool> DeleteAvailableCoursesBySemesterIdAsync(int semesterId);
    }
}
