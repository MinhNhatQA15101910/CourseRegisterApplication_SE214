using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateBack()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async Task NavigateBackToRoot()
        {
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }

        public async Task NavigateTo(Page page)
        {
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
