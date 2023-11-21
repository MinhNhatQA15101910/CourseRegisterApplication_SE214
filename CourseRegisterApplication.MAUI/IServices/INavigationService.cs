namespace CourseRegisterApplication.MAUI.IServices
{
    public interface INavigationService
    {
        Task NavigateTo(Page page);
        Task NavigateBack();
        Task NavigateBackToRoot();
    }
}
