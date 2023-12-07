﻿namespace CourseRegisterApplication.MAUI.IServices
{
    interface IDepartmentService
    {
        Task<Department> AddDepartment(Department department);
        Task<bool> DeleteDepartment(int departmentId);
        Task<List<Department>> GetAllDepartments();
    }
}
