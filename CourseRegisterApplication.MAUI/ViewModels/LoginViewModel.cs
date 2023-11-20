using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels
{
    public partial class LoginViewModel : ObservableObject
	{
		private readonly ContentPage _page;
		private readonly IUserService _userService;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(LoginUserCommand))]
		private string username;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(LoginUserCommand))]
		private string password;

		[ObservableProperty]
		private bool isLoading = false;

		public LoginViewModel(ContentPage page, IUserService userService)
		{
            _page = page;
			_userService = userService;
		}

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

				await _page.DisplayAlert("Success!", "Login Successfully", "OK");

				var navParam = new Dictionary<string, object>();
				navParam.Add("CurrentUser", user);
				switch (user.Role)
				{
					case Role.Admin:
				await _page.Navigation.PushAsync(new AdminFlyoutPage());
                        Clear();
                        break;
					case Role.Accountant:
                        // TODO: Navigate to Accountant Page
                        Clear();
                        break;
                    case Role.Student:
                        // TODO: Navigate to Accountant Page
                        Clear();
                        break;
                }
			}
			else
			{
				await _page.DisplayAlert("Error", "Incorrect Username or Password. Please try again.", "OK");
			}
		}

		private string _usernameLoginMessageText;
		public string UsernameLoginMessageText
		{
			get { return _usernameLoginMessageText; }
			set
			{
				if (_usernameLoginMessageText != value)
				{
					_usernameLoginMessageText = value;
					OnPropertyChanged(nameof(UsernameLoginMessageText));
				}
			}
		}

		private Color _usernameLoginColor;
		public Color UsernameLoginColor
		{
			get { return _usernameLoginColor; }
			set
			{
				if (_usernameLoginColor != value)
				{
					_usernameLoginColor = value;
					OnPropertyChanged(nameof(UsernameLoginColor));
				}
			}
		}

		private string _passwordLoginMessageText;
		public string PasswordLoginMessageText
		{
			get { return _passwordLoginMessageText; }
			set
			{
				if (_passwordLoginMessageText != value)
				{
					_passwordLoginMessageText = value;
					OnPropertyChanged(nameof(PasswordLoginMessageText));
				}
			}
		}

		private Color _passwordLoginColor;
		public Color PasswordLoginColor
		{
			get { return _passwordLoginColor; }
			set
			{
				if (_passwordLoginColor != value)
				{
					_passwordLoginColor = value;
					OnPropertyChanged(nameof(PasswordLoginColor));
				}
			}
		}

		public static int globalVariable = 0;
		public static int globalVariable2 = 0;

		public bool CanLoginUser()
		{
			int index = 0;
			int index2 = 0;
			if (string.IsNullOrEmpty(Username))
			{
				if (globalVariable == 0)
				{
					UsernameLoginColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					UsernameLoginColor = Color.FromArgb("#BF1D28");
				}
				UsernameLoginMessageText = "Username cannot be empty.";
				index++;
			}
			else
			{
				globalVariable = 1;
				UsernameLoginColor = Color.FromArgb("#BF1D28");
				if (Username.Length < 3)
				{
					UsernameLoginMessageText = "Your username must be at least 3 characters.";
					index++;
				}
				else
				{
					UsernameLoginColor = Color.FromArgb("#007D3A");
					UsernameLoginMessageText = "Valid Username.";
					index = 0;
				}
			}
			if (string.IsNullOrEmpty(Password))
			{
				if (globalVariable2 == 0)
				{
					PasswordLoginColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					PasswordLoginColor = Color.FromArgb("#BF1D28");
				}
				PasswordLoginMessageText = "Password cannot be empty.";
				index2++;
			}
			else
			{
				globalVariable2 = 1;
				PasswordLoginColor = Color.FromArgb("#BF1D28");
				if(Password.Length < 8 ) 
				{
					PasswordLoginMessageText = "Your password must be at least 8 characters.";
					index2++;
				}
                else
                {
					PasswordLoginColor = Color.FromArgb("#007D3A");
					PasswordLoginMessageText = "Valid Password.";
					index2 = 0;
				}
			}
			if (index > 0 || index2>0) return false;
			else return true;
		}

		private void Clear()
		{
			IsLoading = false;
            Username = "";
            Password = "";
        }
    }
}
