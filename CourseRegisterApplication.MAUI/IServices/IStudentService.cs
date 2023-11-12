namespace CourseRegisterApplication.MAUI.IServices
{
    internal interface IStudentService 
    {
        Task<Student> GetAllStudent();
        Task<Student> GetStudentBySpecificID(int id);
    }
}
