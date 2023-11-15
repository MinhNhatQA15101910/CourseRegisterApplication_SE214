using CourseRegisterApplication.MAUI.IServices;
using CourseRegisterApplication.MAUI.Services;
using CourseRegisterApplication.MAUI.Views;
using CourseRegisterApplication.MAUI.Views.AdminViews;

namespace CourseRegisterApplication.MAUI.ViewModels.AdminViewModels
{
    public partial class AddUpdateAccountViewModel : ObservableObject
    {
        public readonly AddUpdateAccountPage _addUpdateAccountPage;
		private readonly IUserService _userService = new UserService();

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(AddUpdateAccountCommand))]
		private string username;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(AddUpdateAccountCommand))]
		private string password;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(AddUpdateAccountCommand))]
		private string email;

		[ObservableProperty]
		private bool isLoading = false;

		public AddUpdateAccountViewModel(AddUpdateAccountPage addUpdateAccountPage)
		{
			_addUpdateAccountPage = addUpdateAccountPage;
		}

		[RelayCommand(CanExecute = nameof(CanAddUpdateAccount))]
		public async Task AddUpdateAccount()
		{
			IsLoading = true;
			User user = await _userService.AddUpdateAccount(Username ,Helpers.EncryptData(Password), Email);
		}

		private string _usernameMessageText;
		public string UsernameMessageText
		{
			get { return _usernameMessageText; }
			set
			{
				if (_usernameMessageText != value)
				{
					_usernameMessageText = value;
					OnPropertyChanged(nameof(UsernameMessageText));
				}
			}
		}

		private Color _usernameColor;
		public Color UsernameColor
		{
			get { return _usernameColor; }
			set
			{
				if (_usernameColor != value)
				{
					_usernameColor = value;
					OnPropertyChanged(nameof(UsernameColor));
				}
			}
		}

		private string _passwordMessageText;
		public string PasswordMessageText
		{
			get { return _passwordMessageText; }
			set
			{
				if (_passwordMessageText != value)
				{
					_passwordMessageText = value;
					OnPropertyChanged(nameof(PasswordMessageText));
				}
			}
		}

		private Color _passwordColor;
		public Color PasswordColor
		{
			get { return _passwordColor; }
			set
			{
				if (_passwordColor != value)
				{
					_passwordColor = value;
					OnPropertyChanged(nameof(PasswordColor));
				}
			}
		}

		private string _emailMessageText;
		public string EmailMessageText
		{
			get { return _emailMessageText; }
			set
			{
				if (_emailMessageText != value)
				{
					_emailMessageText = value;
					OnPropertyChanged(nameof(EmailMessageText));
				}
			}
		}

		private Color _emailColor;
		public Color EmailColor
		{
			get { return _emailColor; }
			set
			{
				if (_emailColor != value)
				{
					_emailColor = value;
					OnPropertyChanged(nameof(EmailColor));
				}
			}
		}

		public static int globalVariable = 0;
		public static int globalVariable2 = 0;
		public static int globalVariable3 = 0;

		public bool CanAddUpdateAccount()
		{
			int index = 0;
			int index2 = 0;
			int index3 = 0;
			if (string.IsNullOrEmpty(Username))
			{
				if (globalVariable == 0)
				{
					UsernameColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					UsernameColor = Color.FromArgb("#BF1D28");
				}
				UsernameMessageText = "Username cannot be empty.";
				index++;
			}
			else
			{
				globalVariable = 1;
				UsernameColor = Color.FromArgb("#BF1D28");
				if (Username.Length < 3)
				{
					UsernameMessageText = "Your username must be at least 3 characters.";
					index++;
				}
				else
				{
					UsernameColor = Color.FromArgb("#007D3A");
					UsernameMessageText = "Valid Username.";
					index = 0;
				}
			}
			if (string.IsNullOrEmpty(Password))
			{
				if (globalVariable2 == 0)
				{
					PasswordColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					PasswordColor = Color.FromArgb("#BF1D28");
				}
				PasswordMessageText = "Password cannot be empty.";
				index2++;
			}
			else
			{
				globalVariable2 = 1;
				PasswordColor = Color.FromArgb("#BF1D28");
				if (Password.Length < 8)
				{
					PasswordMessageText = "Your password must be at least 8 characters.";
					index2++;
				}
				else
				{
					PasswordColor = Color.FromArgb("#007D3A");
					PasswordMessageText = "Valid Password.";
					index2 = 0;
				}
			}
			if (string.IsNullOrEmpty(Email))
			{
				if (globalVariable3 == 0)
				{
					EmailColor = Color.FromArgb("#FFFFFF");
				}
				else
				{
					EmailColor = Color.FromArgb("#BF1D28");
				}
				EmailMessageText = "Email cannot be empty.";
				index3++;
			}
			else
			{
				globalVariable3 = 1;
				EmailColor = Color.FromArgb("#BF1D28");
				if (Email.Length < 8)
				{
					EmailMessageText = "Your email must be at least 8 characters.";
					index3++;
				}
				else
				{
					EmailColor = Color.FromArgb("#007D3A");
					EmailMessageText = "Valid Email.";
					index3 = 0;
				}
			}

			if (index > 0 || index2 > 0 || index3 > 0) return false;
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
