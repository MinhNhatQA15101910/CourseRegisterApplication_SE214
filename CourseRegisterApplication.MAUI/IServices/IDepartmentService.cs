namespace CourseRegisterApplication.MAUI.IServices
{
    interface IDepartmentService
    {
        Task<List<Department>> GetDepartments();
        Task<bool> DeleteDepartment(int departmentId);
    }
}
