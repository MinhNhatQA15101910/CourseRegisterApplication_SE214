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
					case "Admin":
                        var navParam = new Dictionary<string, object>();
                        navParam.Add("CurrentUser", user);
                        await _loginPage.Navigation.PushAsync(new AdminFlyoutPage());
                        Clear();
                        break;
					case "Accountant":
                        // TODO: Navigate to Accountant Page
                        Clear();
                        break;
                    case "Student":
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

		private bool _emptyUsernameMessage;
		private bool _shortUsernameMessage;
		private bool _validUsernameMessage;
		private bool _emptyPasswordMessage;
		private bool _shortPasswordMessage;
		private bool _validPasswordMessage;

		public bool EmptyUsernameMessage
		{
			get { return _emptyUsernameMessage; }
			set
			{
				if (_emptyUsernameMessage != value)
				{
					_emptyUsernameMessage = value;
					OnPropertyChanged(nameof(EmptyUsernameMessage));
				}
			}
		}
		public bool EmptyPasswordMessage
		{
			get { return _emptyPasswordMessage; }
			set
			{
				if (_emptyPasswordMessage != value)
				{
					_emptyPasswordMessage = value;
					OnPropertyChanged(nameof(EmptyPasswordMessage));
				}
			}
		}
		public bool ShortPasswordMessage
		{
			get { return _shortPasswordMessage; }
			set
			{
				if (_shortPasswordMessage != value)
				{
					_shortPasswordMessage = value;
					OnPropertyChanged(nameof(ShortPasswordMessage));
				}
			}
		}
		public bool ValidPasswordMessage
		{
			get { return _validPasswordMessage; }
			set
			{
				if (_validPasswordMessage != value)
				{
					_validPasswordMessage = value;
					OnPropertyChanged(nameof(ValidPasswordMessage));
				}
			}
		}
		public bool ShortUsernameMessage
		{
			get { return _shortUsernameMessage; }
			set
			{
				if (_shortUsernameMessage != value)
				{
					_shortUsernameMessage = value;
					OnPropertyChanged(nameof(ShortUsernameMessage));
				}
			}
		}
		public bool ValidUsernameMessage
		{
			get { return _validUsernameMessage; }
			set
			{
				if (_validUsernameMessage != value)
				{
					_validUsernameMessage = value;
					OnPropertyChanged(nameof(ValidUsernameMessage));
				}
			}
		}

		private Color _color;
		private Color _color2;

		public Color Color
		{
			get { return _color; }
			set
			{
				if (_color != value)
				{
					_color = value;
					OnPropertyChanged(nameof(Color));
				}
			}
		}
		public Color Color2
		{
			get { return _color2; }
			set
			{
				if (_color2 != value)
				{
					_color2 = value;
					OnPropertyChanged(nameof(Color2));
				}
			}
		}

		public static int globalVariable = 0;
		public static int globalVariable2 = 0;

		public bool CanLoginUser()
		{
			int index = 0;
			int index2 = 0;
			if(globalVariable == 0)
			{
				Color = Color.FromArgb("#FFFFFF");
			}
			if (globalVariable2 == 0)
			{
				Color2 = Color.FromArgb("#FFFFFF");
			}
			if (string.IsNullOrEmpty(Username))
			{
				EmptyUsernameMessage = true;
				ValidUsernameMessage = false;
				ShortUsernameMessage = false;
				index++;
			}
			else
			{
				globalVariable = 1;
				Color =Color.FromArgb("#BF1D28");
				EmptyUsernameMessage = false;
				if (Username.Length < 3)
				{
					ShortUsernameMessage = true;
					ValidUsernameMessage = false;
					index++;
				}
				else
				{
					ShortUsernameMessage = false;
					ValidUsernameMessage = true;
					index = 0;
				}
			}
			if (string.IsNullOrEmpty(Password))
			{
				EmptyPasswordMessage = true;
				ValidPasswordMessage = false;
				ShortPasswordMessage = false;
				index2++;
			}
			else
			{
				globalVariable2 = 1;
				Color2 = Color.FromArgb("#BF1D28");
				EmptyPasswordMessage = false;
				if(Password.Length < 8 ) 
				{
					ShortPasswordMessage = true;
					ValidPasswordMessage = false;
					index2++;
				}
                else
                {
					ShortPasswordMessage = false;
					ValidPasswordMessage = true;
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
