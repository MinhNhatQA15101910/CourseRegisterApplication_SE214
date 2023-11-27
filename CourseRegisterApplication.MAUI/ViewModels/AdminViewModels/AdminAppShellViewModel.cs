using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AdminAppShellViewModel : ObservableObject
    {
        [RelayCommand]
        public async Task NavigateToChangePasswordPage()
        {
            if (Shell.Current.CurrentPage is not ChangePasswordPage)
            {
                await Shell.Current.GoToAsync(nameof(ChangePasswordPage), true);
            }
        }
    }
}
