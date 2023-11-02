using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseRegisterApplication_SE214.Models;
using CourseRegisterApplication_SE214.Services;
using CourseRegisterApplication_SE214.Views;
using CourseRegisterApplication_SE214.Views.AdminViews;

namespace CourseRegisterApplication_SE214.ViewModels
{
	public partial class LoginViewModel : ObservableObject
	{
		private readonly LoginPage _loginPage;
		private readonly ILoginServices _loginService = new LoginServices();

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
			User user = await _loginService.LoginUser(Username, Helpers.EncryptData(Password));
			IsLoading = false;
			if (user != null)
			{
				Username = "";
				Password = "";

				var navParam = new Dictionary<string, object>();
				navParam.Add("CurrentUser", user);
				await _loginPage.Navigation.PushAsync(new AdminFlyoutPage());
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
	}
}
