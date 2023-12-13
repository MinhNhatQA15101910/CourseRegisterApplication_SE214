using CourseRegisterApplication.MAUI.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.Services
{
    public class StudentService : IStudentService
    {
		private readonly HttpClient _httpClient;

		public StudentService(HttpClient httpClient)
        {
			_httpClient = httpClient;
        }

        public Task<List<Student>> GetStudents()
        {
            throw new NotImplementedException();
        }
    }
}
