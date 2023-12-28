namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudentsByBranchId(int branchId);
        Task<List<Student>> GetStudentsByDistrictId(int districtId);
        Task<List<Student>> GetAllStudents();
        Task<Student> AddStudent(Student student);
    }
}
