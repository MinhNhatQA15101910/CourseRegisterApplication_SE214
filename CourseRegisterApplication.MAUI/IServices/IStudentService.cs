using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudents();
        Task<Student> AddStudent(Student student);
    }
}
