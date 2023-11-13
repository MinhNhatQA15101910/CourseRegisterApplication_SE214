namespace CourseRegisterApplication.MAUI.IServices
{
    internal interface IStudentService 
    {
        Task<List<Student>> GetAllStudent();
    }
}
