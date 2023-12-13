using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class StudentService : IStudentService
    {
		private readonly HttpClient _httpClient;

		public StudentService(HttpClient httpClient)
        {
			_httpClient = httpClient;
        }

        public Task<Student> AddStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public Task<List<Student>> GetStudents()
        {
            throw new NotImplementedException();
        }
    }
}
