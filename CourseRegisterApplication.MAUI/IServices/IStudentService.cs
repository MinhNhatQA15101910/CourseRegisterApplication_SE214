﻿namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IStudentService
    {
        Task<List<Student>> GetStudents();

        Task<Student> AddStudent(Student student);
    }
}
