using CourseRegisterApplication.MAUI.Views;

namespace CourseRegisterApplication.MAUI.ViewModels.AccountantViewModels
{
    public partial class AccountantAppShellViewModel : ObservableObject
	{
		[ObservableProperty]
		private string username = GlobalConfig.CurrentUser.Username;

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
