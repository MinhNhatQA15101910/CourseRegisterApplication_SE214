using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AdminAppShellViewModel : ObservableObject
    {
        public AdminAppShellViewModel() { }

        [RelayCommand]
        public async Task ChangePassword()
        {
            if (Shell.Current.CurrentState.ToString() != nameof(ChangePasswordPage))
            {
                await Shell.Current.GoToAsync(nameof(ChangePasswordPage), true);
            }
        }
    }
}
