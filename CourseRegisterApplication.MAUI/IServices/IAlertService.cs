namespace CourseRegisterApplication.MAUI.IServices
{
    public interface IAlertService
    {
        Task DisplayAlert(string title, string message, string cancel);
    }
}
