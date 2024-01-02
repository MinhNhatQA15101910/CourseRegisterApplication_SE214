namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IStudentService
    {
        Task<List<Student>> GetAllStudents();
        Task<List<Student>> GetFullInformationOfAllStudents();
        Task<Student> GetStudentBySpecificId(string studentSpecificId);
        Task<Student> GetFullInformationOfStudentBySpecificId(string studentSpecificId);
        Task<List<Student>> GetStudentsByBranchId(int branchId);
        Task<List<Student>> GetStudentsByDistrictId(int districtId);
        Task<Student> AddStudent(Student student, List<PriorityType> priorityTypes);
        Task<bool> UpdateImageUrl(Student student, string newImageUrl);
        Task<bool> UpdateStudent(int studentId, Student student, List<PriorityType> priorityTypes);
    }
}
