using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AdminViews;
using CourseRegisterApplication.MAUI.Views.StudentViews;
using CourseRegisterApplication.MAUI.Views.AccountantViews;

namespace CourseRegisterApplication.MAUI.ViewModels
{
    public partial class LoginViewModel : ObservableObject
	{
		#region Services
		private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        #endregion

        #region Variables 
        private int globalVariable1 = 0;
        private int globalVariable2 = 0;
        #endregion

        #region Binding Data
		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(LoginUserCommand))]
		private string username;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(LoginUserCommand))]
		private string password;
        #endregion

        #region Binding UI
		[ObservableProperty]
        private string usernameMessage;

        [ObservableProperty]
        private Color usernameMessageColor;

        [ObservableProperty]
        private string passwordMessage;

        [ObservableProperty]
        private Color passwordMessageColor;

        [ObservableProperty]
		private bool isLoading = false;
        #endregion

        #region Constructor
        public LoginViewModel(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_userService = serviceProvider.GetService<IUserService>();
		}
        #endregion

        #region LoginUserCommand
		[RelayCommand(CanExecute = nameof(CanLoginUser))]
		public async Task LoginUser()
		{
			IsLoading = true;
			User user = await _userService.LoginUser(Username, Helpers.EncryptData(Password));
			IsLoading = false;
			if (user != null)
			{
				Username = "";
				Password = "";
				GlobalConfig.CurrentUser = user;

				await Application.Current.MainPage.DisplayAlert("Success!", "Login Successfully", "OK");

				switch (user.Role)
				{
					case Role.Admin:
                        Application.Current.MainPage = _serviceProvider.GetService<AdminAppShell>();
                        Clear();
                        break;
					case Role.Accountant:
						Application.Current.MainPage = _serviceProvider.GetService<AccountantAppShell>();
						Clear();
                        break;
                    case Role.Student:
						Application.Current.MainPage = _serviceProvider.GetService<StudentAppShell>();
						Clear();
						break;
				}
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Incorrect Username or Password. Please try again.", "OK");
			}
		}

		public bool CanLoginUser()
		{
			int index = 0;
			int index2 = 0;
			if (string.IsNullOrEmpty(Username))
			{
				if (globalVariable1 == 0)
				{
					UsernameMessageColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
                    UsernameMessageColor = Color.FromArgb("#BF1D28");
				}
				UsernameMessage = "Username cannot be empty.";
				index++;
			}
			else
			{
                globalVariable1 = 1;
                UsernameMessageColor = Color.FromArgb("#BF1D28");
				if (Username.Length < 3)
				{
                    UsernameMessage = "Your username must be at least 3 characters.";
					index++;
				}
				else
				{
                    UsernameMessageColor = Color.FromArgb("#007D3A");
                    UsernameMessage = "Valid Username.";
					index = 0;
				}
			}
			if (string.IsNullOrEmpty(Password))
			{
				if (globalVariable2 == 0)
				{
					PasswordMessageColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
                    PasswordMessageColor = Color.FromArgb("#BF1D28");
				}
				PasswordMessage = "Password cannot be empty.";
				index2++;
			}
			else
			{
				globalVariable2 = 1;
                PasswordMessageColor = Color.FromArgb("#BF1D28");
				if(Password.Length < 8 ) 
				{
					PasswordMessage = "Your password must be at least 8 characters.";
					index2++;
				}
                else
			{
                    PasswordMessageColor = Color.FromArgb("#007D3A");
                    PasswordMessage = "Valid Password.";
					index2 = 0;
				}
			}

			if (index > 0 || index2 > 0)
			{
				return false;
			}

			return true;
		}
        #endregion

        #region Helpers
        private void Clear()
		{
            Username = "";
            Password = "";
			globalVariable1 = 0;
			globalVariable2 = 0;
			IsLoading = false;
        }
        #endregion
	}
}
