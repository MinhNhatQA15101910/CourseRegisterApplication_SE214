namespace CourseRegisterApplication.MAUI.IServices
{
    interface IDepartmentService
    {
        Task<Department> AddDepartment(Department department);
        Task<bool> DeleteDepartment(int departmentId);
        Task<List<Department>> GetAllDepartments();
        Task<Department> GetDepartment(int departmentId);
        Task<bool> UpdateDepartment(int departmentId, Department department);
    }
}
