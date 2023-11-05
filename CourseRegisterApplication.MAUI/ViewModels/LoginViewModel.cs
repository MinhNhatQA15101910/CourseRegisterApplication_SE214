using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels
{
    public partial class LoginViewModel : ObservableObject
	{
		private readonly LoginPage _loginPage;
		private readonly IUserService _userService = new UserService();

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(LoginUserCommand))]
		private string username;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(LoginUserCommand))]
		private string password;

		[ObservableProperty]
		private bool isLoading = false;

		public LoginViewModel(LoginPage loginPage)
		{
			_loginPage = loginPage;
		}

		[RelayCommand(CanExecute = nameof(CanLoginUser))]
		public async Task LoginUser()
		{
			IsLoading = true;
			User user = await _userService.LoginUser(Username, Helpers.EncryptData(Password));
			if (user != null)
			{
				await _loginPage.DisplayAlert("Success!", "Login Successfully", "OK");

				switch (user.Role.RoleName)
				{
					case RoleName.Admin:
                        var navParam = new Dictionary<string, object>();
                        navParam.Add("CurrentUser", user);
                        await _loginPage.Navigation.PushAsync(new AdminFlyoutPage());
                        Clear();
                        break;
					case RoleName.Accountant:
                        // TODO: Navigate to Accountant Page
                        Clear();
                        break;
                    case RoleName.Student:
                        // TODO: Navigate to Accountant Page
                        Clear();
                        break;
                }
			}
			else
			{
				await _loginPage.DisplayAlert("Error", "Incorrect Username or Password. Please try again.", "OK");
			}
		}

		public bool CanLoginUser()
		{
			if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
			{
				return false;
			}

			if (Password.Length < 8)
			{
				return false;
			}

			return true;
		}

		private void Clear()
		{
			IsLoading = false;
            Username = "";
            Password = "";
        }
	}
}
